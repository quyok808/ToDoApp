using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace NCQ.Infrastructure.Shared
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSharedServices(this IApplicationBuilder app,
            ServiceProvider provider)
        {
            var env = provider.GetRequiredService<IHostEnvironment>();

            const string HealthyPathApi = "/todo/health";
            app.UseHealthChecks(HealthyPathApi, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseRouting();
        }
    }
}
