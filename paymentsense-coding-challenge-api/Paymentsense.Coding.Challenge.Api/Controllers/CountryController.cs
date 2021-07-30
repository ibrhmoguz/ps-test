using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Interfaces;
using Paymentsense.Coding.Challenge.Api.Models;

namespace Paymentsense.Coding.Challenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }


        [HttpGet, Route("List")]
        public async Task<ActionResult<PaginatedResult>> Get([FromQuery]PageInfo pageInfo)
        {
            var countryList = await _countryService.GetCountries(pageInfo);

            return Ok(countryList);
        }
    }
}