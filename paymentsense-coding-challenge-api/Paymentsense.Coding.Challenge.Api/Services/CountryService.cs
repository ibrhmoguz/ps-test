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
        private readonly ISimpleCache<List<Country>> _simpleCache;
        private const string CountryKey = "Countries";

        public CountryService(ICountryClient countryClient, ISimpleCache<List<Country>> simpleCache)
        {
            _countryClient = countryClient;
            _simpleCache = simpleCache;
        }

        public async Task<List<Country>> GetCountries(PageInfo pageInfo)
        {
            var countries = _simpleCache.Get(CountryKey);
            if (countries == null)
            {
                countries = await _countryClient.GetCountries();
                if (countries != null && countries.Any())
                {
                    _simpleCache.Set(CountryKey, countries);
                }
                else
                {
                    return null;
                }
            }

            if (pageInfo == null)
            {
                return countries;
            }

            pageInfo.Page ??= 1;
            pageInfo.PageNumber ??= 1;

            var skipCount = pageInfo.Page.Value * pageInfo.PageNumber.Value;
            return pageInfo.PageNumber == 1
                ? countries.Take(pageInfo.Page.Value).ToList()
                : countries.Skip(skipCount).Take(pageInfo.Page.Value).ToList();
        }
    }
}