using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Paymentsense.Coding.Challenge.Api.Controllers;
using Paymentsense.Coding.Challenge.Api.Interfaces;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Tests.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Controllers
{
    public class CountryControllerTests : IClassFixture<CountryListFixture>
    {
        private readonly CountryListFixture _countryListFixture;
        private readonly Mock<ICountryService> _countryServiceMock;

        public CountryControllerTests(CountryListFixture countryListFixture)
        {
            _countryListFixture = countryListFixture;
            _countryServiceMock = new Mock<ICountryService>();
        }

        [Fact]
        public async void Get_OnInvoke_ReturnsCountryList()
        {
            var paginatedResult = new PaginatedResult
            {
                Countries = _countryListFixture.Countries,
            };
            _countryServiceMock.Setup(c => c.GetCountries(null)).ReturnsAsync(() => paginatedResult);
            var controller = new CountryController(_countryServiceMock.Object);

            var result = (await controller.Get(null)).Result as OkObjectResult;

            _countryServiceMock.Verify(c => c.GetCountries(null), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<PaginatedResult>();
            result.Value.Should().Be(paginatedResult);
        }
    }
}
