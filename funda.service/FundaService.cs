using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using RateLimiter;
using ComposableAsync;
using System.ServiceModel.Security;

namespace funda.service
{
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

        /// <summary>
        /// Fetches all objects from the funda API that match the query given by <paramref name="zo"/>.
        /// </summary>
        /// <param name="zo">A search path like "/amsterdam"</param>
        /// <returns>List of objects</returns>
        public async Task<IEnumerable<Object1>> GetAllObjects(string zo) {
            var all = new List<Object1>();
            var totalPages = -1;
            var pageIndex = 1;
            var pageSize = 20;
            var totalObjects = -1;
            var retryMax = 4;
            var retryAttempt = 0;
            var retryDelay = TimeSpan.FromSeconds(15);
            var client = GetClient();
            do
            {
                await _rateLimiter;
                try
                {
                    _logger.LogDebug($"loading page {pageIndex}...");
                    var response = await client.ZoekAanbodAsync(new ZoekAanbodRequest
                    {
                        key = _options.ApiKey,
                        aanbodType = "koop",
                        zoekPad = zo,
                        page = $"{pageIndex}",
                        pagesize = $"{pageSize}"
                    });
                    if (totalObjects != -1 && totalObjects != response.ZoekAanbodResult.TotaalAantalObjecten)
                        _logger.LogWarning($"number of objects changed while fetching, previous={totalObjects}, current={response.ZoekAanbodResult.TotaalAantalObjecten}");
                    totalObjects = response.ZoekAanbodResult.TotaalAantalObjecten;
                    totalPages = (int)Math.Ceiling((double)totalObjects / pageSize);
                    _logger.LogDebug($"loaded page {pageIndex}, totalPages={totalPages}, # objects={response.ZoekAanbodResult.Objects.Length}");
                    all.AddRange(response.ZoekAanbodResult.Objects);
                    pageIndex++;
                    retryAttempt = 0;
                } 
                catch (MessageSecurityException ex)
                {
                    if (retryAttempt++ < retryMax) {
                        _logger.LogWarning($"Error {ex.Message}, retrying after {retryDelay}...");
                        await Task.Delay(retryDelay);
                    } else {
                        _logger.LogError(ex, $"Error loading page {pageIndex}, retried {retryAttempt-1} times");
                        throw;
                    }
                }
            } while (totalPages == -1 || pageIndex <= totalPages);
            if (all.Count != totalObjects)
                _logger.LogWarning($"Got different total than expected: all.Count={all.Count}, totalObjects={totalObjects}");
            return all;
        }
    }
}
