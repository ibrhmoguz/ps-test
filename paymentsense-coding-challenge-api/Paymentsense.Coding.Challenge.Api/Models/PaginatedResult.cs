using System.Collections.Generic;

namespace Paymentsense.Coding.Challenge.Api.Models
{
    public class PaginatedResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<Country> Countries { get; set; }
    }
}
