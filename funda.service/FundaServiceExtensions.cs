using Microsoft.Extensions.DependencyInjection;
using funda.service;

namespace funda
{
    public static class FundaServiceExtensions {
        public static void AddFundaService(this IServiceCollection services) {
            services.AddSingleton<IFundaService, FundaService>();
            services.AddSingleton<IAanbod>(serviceProvider => new AanbodClient(AanbodClient.EndpointConfiguration.wshttp));
        }
    }
}
