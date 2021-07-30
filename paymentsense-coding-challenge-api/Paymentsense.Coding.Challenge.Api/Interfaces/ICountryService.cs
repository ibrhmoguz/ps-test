using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Models;

namespace Paymentsense.Coding.Challenge.Api.Interfaces
{
    public interface ICountryService
    {
        Task<PaginatedResult> GetCountries(PageInfo pageInfo);
    }
}