using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NCQ.Infrastructure.Shared.HealthChecks
{
    public static class HealthCheckServiceRegister
    {
        public static void AddCustomHealthCheck(this IServiceCollection services,
             IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();
        }
    }
}
