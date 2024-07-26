using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CountriesTestApplication.Data.Entities;
using CountriesTestApplication.Data.Entities.Dictionary;
using CountriesTestApplication.Infraestructure.Services.Definitions;
using CountriesTestApplication.Infraestructure.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace CountriesTestApplication.Infraestructure.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly IConfiguration _configuration;
        private readonly RestClient _client;
        private readonly IMemoryCache _cache;

        public CountryService(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _client =  new RestClient(_configuration.GetSection("RestCountriesAPI").Value);
            _cache = cache;
        }

        public async Task<PagedResult<Country>> GetCountries(int page, int pageSize, string region = null, string language = null, string search = null, string sortBy = null, bool descending = false)
        {
            var cacheKey = $"countries-{region}-{language}-{search}-{sortBy}-{descending}";
            if (!_cache.TryGetValue(cacheKey, out List<Country> countries))
            {
                countries = await FetchAllCountries();
                if (countries.Count() > 0)
                {
                    _cache.Set(cacheKey, countries, TimeSpan.FromMinutes(30));
                }
            }

            if (!string.IsNullOrEmpty(region))
            {
                countries = countries.Where(c => c.Region.Equals(region, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(language))
            {
                countries = countries.Where(c => c.Languages != null && c.Languages.Values.Any(l => l.Equals(language, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                countries = countries.Where(c => c.Name.Common.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                  c.Name.Official.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        countries = descending ? countries.OrderByDescending(c => c.Name.Common).ToList() : countries.OrderBy(c => c.Name.Common).ToList();
                        break;
                    case "region":
                        countries = descending ? countries.OrderByDescending(c => c.Region).ToList() : countries.OrderBy(c => c.Region).ToList();
                        break;

                    default:
                        break;
                }
            }

            return countries.GetPaged(page, pageSize);
           
        }

        public async Task<Country> GetCountryByCode(string code)
        {
            var cacheKey = $"country_{code}";
            if (!_cache.TryGetValue(cacheKey, out Country country))
            {
                country = await FetchCountryByCode(code);
                _cache.Set(cacheKey, country, TimeSpan.FromMinutes(30));
            }
            return country;
        }

        public async Task<Dictionary<string, List<string>>> GetLanguages()
        {
            var cacheKey = "languages";
            if (!_cache.TryGetValue(cacheKey, out Dictionary<string, List<string>> languages))
            {
                List<Country> countriesList = await FetchAllCountries();
                languages = countriesList.Where(c => c.Languages != null && c.Languages.Any()) 
                                         .SelectMany(c => c.Languages.Select(l => new { Language = l.Value.ToString(), Country = c.Name.Common }))
                                         .GroupBy(x => x.Language)
                                         .ToDictionary(g => g.Key, g => g.Select(c => c.Country).ToList());
                _cache.Set(cacheKey, languages, TimeSpan.FromMinutes(30));
            }
            return languages;
        }


        public async Task<Dictionary<string, List<string>>> GetRegions()
        {
            try
            {
                var cacheKey = "regions";
                if (!_cache.TryGetValue(cacheKey, out Dictionary<string, List<string>> regions))
                {
                    List<Country> countriesList = await FetchAllCountries();
                    regions = countriesList.GroupBy(c => c.Region)
                                           .ToDictionary(g => g.Key,
                                           g => g.Select(c => c.Name.Common).ToList());
                    _cache.Set(cacheKey, regions, TimeSpan.FromMinutes(30));
                }
                return regions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<Country>> FetchAllCountries()
        {
            try
            {
                var request = new RestRequest("all", Method.Get);
                var response = await _client.ExecuteAsync<List<Country>>(request);
                if (response.IsSuccessful)
                {
                    var countries = JsonConvert.DeserializeObject<List<Country>>(response.Content);
                    return countries ?? new List<Country>();

                }
                else
                {
                    throw new Exception("Error fetching countries");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    

        private async Task<Country> FetchCountryByCode(string code)
        {
            try
            {
                var request = new RestRequest($"alpha/{code}", Method.Get);
                var response = await _client.ExecuteAsync<Country>(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error fetching country data or country not found.");
                }

                var country = JsonConvert.DeserializeObject<List<Country>>(response.Content)?.FirstOrDefault();

                if (country == null)
                {
                    throw new Exception("Country not found or data is invalid.");
                }
                return country;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
