using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using funda.Services;
using System.Collections.Generic;
using System.Linq;
using RateLimiter;
using ComposableAsync;

namespace funda
{
    public class FundaServiceException : Exception {
        public FundaServiceException(Exception ex) {
        }
    }
    public interface IFundaService {
        IAanbod GetClient();
        Task<IEnumerable<Object1>> GetAllObjects(string zo);
    }
    public class FundaService : IFundaService {
        private readonly ILogger<FundaService> _logger;
        private readonly IAanbod _client;
        private readonly FundaServiceOptions _options;
        private readonly TimeLimiter _rateLimiter = RateLimiter.TimeLimiter.GetFromMaxCountByInterval(100, TimeSpan.FromMinutes(1));

        public FundaService(ILogger<FundaService> logger, IOptions<FundaServiceOptions> options, IAanbod client)
        {
            _client = client;
            _logger = logger;
            _options = options.Value;
        }

        public IAanbod GetClient() {
            return _client;
        }

        public async Task<IEnumerable<Object1>> GetAllObjects(string zo) {
            var all = new List<Object1>();
            var totalPages = 0;
            var pageIndex = 0;
            var pageSize = 25;
            var client = GetClient();
            do
            {
                await _rateLimiter;
                var response = await client.ZoekAanbodAsync(new ZoekAanbodRequest
                {
                    key = _options.ApiKey,
                    aanbodType = "koop",
                    zoekPad = zo,
                    page = $"{pageIndex}",
                    pagesize = $"{pageSize}"
                });
                totalPages = response.ZoekAanbodResult.TotaalAantalObjecten / pageSize;
                all.AddRange(response.ZoekAanbodResult.Objects);
            } while (pageIndex++ < totalPages);
            return all;
        }
    }
}
