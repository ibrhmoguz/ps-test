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

        public async Task<PaginatedResult> GetCountries(PageInfo pageInfo)
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
                    return new PaginatedResult { PageSize = 0, Page = 1, Countries = null };
                }
            }

            if (pageInfo == null)
            {
                return new PaginatedResult { PageSize = 10, Page = 1, Countries = countries, Total = countries.Count };
            }
            
            var skipCount = pageInfo.Page * pageInfo.PageSize;
            var result = pageInfo.Page == 1
                ? countries.Take(pageInfo.PageSize).ToList()
                : countries.Skip(skipCount).Take(pageInfo.PageSize).ToList();

            return new PaginatedResult
            {
                PageSize = pageInfo.PageSize,
                Page = pageInfo.Page,
                Countries = result,
                Total = countries.Count
            };
        }
    }
}