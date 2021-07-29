using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Paymentsense.Coding.Challenge.Api.Interfaces;
using Paymentsense.Coding.Challenge.Api.Models;

namespace Paymentsense.Coding.Challenge.Api.Client
{
    public class CountryClient : ICountryClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public CountryClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = clientFactory.CreateClient();
        }

        public async Task<List<Country>> GetCountries()
        {
            // Get country api url from config
            var countryApiUrl = _configuration.GetValue<string>("CountryApiUrl");
            if (string.IsNullOrEmpty(countryApiUrl))
            {
                throw new ApplicationException("CountryApiUrl missing in configuration file");
            }

            var response = await _client.GetAsync(countryApiUrl);
            if (response == null || !response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Country>>(content);
        }
    }
}