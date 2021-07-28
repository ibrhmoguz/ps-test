using System.Collections.Generic;
using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Models;

namespace Paymentsense.Coding.Challenge.Api.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetCountries(PageInfo pageInfo);
    }
}