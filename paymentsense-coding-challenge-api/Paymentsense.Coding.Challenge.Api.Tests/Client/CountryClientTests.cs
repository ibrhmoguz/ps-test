using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Paymentsense.Coding.Challenge.Api.Client;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Tests.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Client
{
    public class CountryClientTests : IClassFixture<CountryListFixture>
    {
        private readonly CountryListFixture _countryListFixture;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly IConfiguration _badConfiguration;
        private readonly IConfiguration _correctConfiguration;

        public CountryClientTests(CountryListFixture countryListFixture)
        {
            _countryListFixture = countryListFixture;
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _badConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "TestKey", "TestValue" } })
                .Build();

            _correctConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "CountryApiUrl", "https://testUrl.com" } })
                .Build();
        }

        [Fact]
        public async void GetCountries_WrongConfigKey_ReturnsErrorMessage()
        {
            var countryClient = new CountryClient(_httpClientFactoryMock.Object, _badConfiguration);

            await Assert.ThrowsAsync<ApplicationException>(() => countryClient.GetCountries());
        }

        [Fact]
        public async void GetCountries_CorrectConfigKey_ReturnsNullResponse()
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => null)
                .Verifiable();
            var client = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://testUrl.com") };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var countryClient = new CountryClient(_httpClientFactoryMock.Object, _correctConfiguration);

            await Assert.ThrowsAsync<InvalidOperationException>(() => countryClient.GetCountries());
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri("https://testUrl.com")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async void GetCountries_CorrectConfigKey_ReturnsNotSuccessCode()
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                })
                .Verifiable();
            var client = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://testUrl.com") };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var countryClient = new CountryClient(_httpClientFactoryMock.Object, _correctConfiguration);
            var response = await countryClient.GetCountries();

            response.Should().BeNull();
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://testUrl.com")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async void GetCountries_CorrectConfigKey_ReturnsCountryList()
        {
            var responseString = await Task.Run(() => JsonConvert.SerializeObject(_countryListFixture.Countries));
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString)
                })
                .Verifiable();
            var client = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://testUrl.com") };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var countryClient = new CountryClient(_httpClientFactoryMock.Object, _correctConfiguration);
            var result = await countryClient.GetCountries();

            result.Should().BeOfType<List<Country>>();
            result.Should().HaveCount(100);
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://testUrl.com")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
