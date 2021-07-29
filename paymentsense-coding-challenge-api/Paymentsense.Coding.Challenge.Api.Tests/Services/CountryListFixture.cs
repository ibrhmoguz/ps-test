using System.Collections.Generic;
using System.Linq;
using Paymentsense.Coding.Challenge.Api.Models;

namespace Paymentsense.Coding.Challenge.Api.Tests.Services
{
    public class CountryListFixture
    {
        public CountryListFixture()
        {
            var country = new Country
            {
                Capital = "Ankara",
                Currencies = new List<Currency> { new Currency { Code = "TRY", Name = "Turkish Lira", Symbol = "TL" } },
                Languages = new List<Language> { new Language { Name = "TR", NativeName = "Turkish" } },
                Population = 123456,
                Name = "Turkey"
            };

            Countries = Enumerable.Range(1, 10).Select(k => country).ToList();
        }

        public List<Country> Countries { get; set; }
    }
}
