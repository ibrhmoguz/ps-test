using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Interfaces;
using Paymentsense.Coding.Challenge.Api.Models;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryClient _countryClient;

        public CountryService(ICountryClient countryClient)
        {
            _countryClient = countryClient;
        }

        public async Task<List<Country>> GetCountries(PageInfo pageInfo)
        {
            var countries = await _countryClient.GetCountries();

            if (pageInfo == null)
            {
                return countries;
            }

            pageInfo.Page ??= 1;
            pageInfo.PageNumber ??= 1;

            var skipCount = pageInfo.Page.Value * pageInfo.PageNumber.Value;
            return countries.Skip(skipCount).Take(pageInfo.PageNumber.Value).ToList();
        }
    }
}