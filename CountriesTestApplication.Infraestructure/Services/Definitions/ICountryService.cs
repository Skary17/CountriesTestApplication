using CountriesTestApplication.Data.Entities;
using CountriesTestApplication.Infraestructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesTestApplication.Infraestructure.Services.Definitions
{
    public interface ICountryService
    {
        Task<PagedResult<Country>> GetCountries(int page, int pageSize, string region, string language, string search,string sortBy, bool descending);
        Task<Country> GetCountryByCode(string code);
        Task<Dictionary<string, List<string>>> GetRegions();
        Task<Dictionary<string, List<string>>> GetLanguages();
    }
}
