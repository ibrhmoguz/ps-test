using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Services
{
    public class SimpleCacheServiceTests : IClassFixture<CountryListFixture>
    {
        private readonly CountryListFixture _countryListFixture;

        public SimpleCacheServiceTests(CountryListFixture countryListFixture)
        {
            _countryListFixture = countryListFixture;
        }

        [Fact]
        public void Get_ReturnsNull()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var simpleCache = new SimpleCacheService<List<Country>>(memoryCache);
            var result = simpleCache.Get("test");

            result.Should().BeNull();
        }

        [Fact]
        public void Set_ReturnsTrue()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var simpleCache = new SimpleCacheService<List<Country>>(memoryCache);
            var setResult = simpleCache.Set("test", _countryListFixture.Countries);

            setResult.Should().BeTrue();
        }

        [Fact]
        public void Set_ReturnsFalse()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var simpleCache = new SimpleCacheService<List<Country>>(memoryCache);
            var setResult = simpleCache.Set("test", _countryListFixture.Countries);

            setResult.Should().BeTrue();

            var setResult2 = simpleCache.Set("test", _countryListFixture.Countries);

            setResult2.Should().BeFalse();
        }

        [Fact]
        public void Get_ReturnsCountries()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var simpleCache = new SimpleCacheService<List<Country>>(memoryCache);
            var setResult = simpleCache.Set("test", _countryListFixture.Countries);

            setResult.Should().BeTrue();

            var result = simpleCache.Get("test");

            result.Should().BeOfType<List<Country>>();
            result.Should().HaveCount(10);
        }
    }
}
