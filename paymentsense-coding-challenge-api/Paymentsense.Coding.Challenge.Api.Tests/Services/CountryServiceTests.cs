using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Paymentsense.Coding.Challenge.Api.Interfaces;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Services
{
    public class CountryServiceTests : IClassFixture<CountryListFixture>
    {
        private readonly CountryListFixture _countryListFixture;
        private readonly Mock<ICountryClient> _countryClientMock;
        private readonly Mock<ISimpleCache<List<Country>>> _simpleCacheMock;

        public CountryServiceTests(CountryListFixture countryListFixture)
        {
            _countryListFixture = countryListFixture;
            _countryClientMock = new Mock<ICountryClient>();
            _simpleCacheMock = new Mock<ISimpleCache<List<Country>>>();
        }

        [Fact]
        public async void GetCountries_EmptyCacheAndNullApiResponse_ReturnsNull()
        {
            _simpleCacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);
            _countryClientMock.Setup(x => x.GetCountries()).ReturnsAsync(() => null);
            var countryService = new CountryService(_countryClientMock.Object, _simpleCacheMock.Object);

            var result = await countryService.GetCountries(null);

            result.Should().BeNull();
        }

        [Fact]
        public async void GetCountries_EmptyCacheAndEmptyApiResponse_ReturnsNull()
        {
            _simpleCacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);
            _countryClientMock.Setup(x => x.GetCountries()).ReturnsAsync(() => new List<Country>());
            var countryService = new CountryService(_countryClientMock.Object, _simpleCacheMock.Object);

            var result = await countryService.GetCountries(null);

            result.Should().BeNull();
        }

        [Fact]
        public async void GetCountries_ReturnsCountryListFromDataSource_WithoutPagination()
        {
            _simpleCacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);
            _countryClientMock.Setup(x => x.GetCountries()).ReturnsAsync(() => _countryListFixture.Countries);
            var countryService = new CountryService(_countryClientMock.Object, _simpleCacheMock.Object);

            var result = await countryService.GetCountries(null);

            result.Should().BeOfType<List<Country>>();
            result.Should().HaveCount(10);
            _countryClientMock.Verify(c => c.GetCountries(), Times.Once);
            _simpleCacheMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetCountries_ReturnsCountryListFromCache_WithoutPagination()
        {
            _simpleCacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => _countryListFixture.Countries);
            var countryService = new CountryService(_countryClientMock.Object, _simpleCacheMock.Object);

            var result = await countryService.GetCountries(null);

            result.Should().BeOfType<List<Country>>();
            result.Should().HaveCount(10);
            _countryClientMock.Verify(c => c.GetCountries(), Times.Never);
            _simpleCacheMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetCountries_ReturnsCountryListFromCache_WithPageInfoWithoutPage()
        {
            _simpleCacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => _countryListFixture.Countries);
            var countryService = new CountryService(_countryClientMock.Object, _simpleCacheMock.Object);

            var result = await countryService.GetCountries(new PageInfo());

            result.Should().BeOfType<List<Country>>();
            result.Should().HaveCount(1);
            _countryClientMock.Verify(c => c.GetCountries(), Times.Never);
            _simpleCacheMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetCountries_ReturnsCountryListFromCache_WithPageInfoPage2PageNumber3()
        {
            _simpleCacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => _countryListFixture.Countries);
            var countryService = new CountryService(_countryClientMock.Object, _simpleCacheMock.Object);

            var prevResult = await countryService.GetCountries(new PageInfo { Page = 2, PageNumber = 1 });
            var result = await countryService.GetCountries(new PageInfo { Page = 2, PageNumber = 2 });
            var nextResult = await countryService.GetCountries(new PageInfo { Page = 2, PageNumber = 3 });

            prevResult.Should().BeOfType<List<Country>>();
            prevResult.Should().HaveCount(2);
            result.Should().BeOfType<List<Country>>();
            result.Should().HaveCount(2);
            nextResult.Should().BeOfType<List<Country>>();
            nextResult.Should().HaveCount(2);
            _countryClientMock.Verify(c => c.GetCountries(), Times.Never);
            _simpleCacheMock.Verify(c => c.Get(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
