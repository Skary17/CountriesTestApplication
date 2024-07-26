using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CountriesTestApplication.Data.Entities;
using CountriesTestApplication.Infraestructure.Services;
using CountriesTestApplication.Infraestructure.Services.Definitions;

namespace CountriesTestApplication.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string region = null, 
            [FromQuery] string language = null, [FromQuery] string search = null, [FromQuery] string sortBy = null, [FromQuery] bool descending = false)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page and pageSize must be greater than zero.");
                }
                var result = await _countryService.GetCountries(page, pageSize, region, language, search, sortBy, descending);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("countries/{code}")]
        public async Task<IActionResult> GetCountryByCode(string code)
        {
           
            try
            {
                var country = await _countryService.GetCountryByCode(code);
                return Ok(country);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("regions")]
        public async Task<IActionResult> GetRegions()
        {
            var regions = await _countryService.GetRegions();
            return Ok(regions);
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguages()
        {
            var languages = await _countryService.GetLanguages();
            return Ok(languages);
        }
    }

}
