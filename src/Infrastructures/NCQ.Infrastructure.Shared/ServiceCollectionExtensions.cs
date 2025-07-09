using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCQ.Infrastructure.Shared.HealthChecks;

namespace NCQ.Infrastructure.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterSharedServices(this IServiceCollection services,
            IConfiguration configuration)
        {
			// Api Version
			services.AddApiVersioning(delegate (ApiVersioningOptions config)
			{
				config.AssumeDefaultVersionWhenUnspecified = true;
				config.DefaultApiVersion = new ApiVersion(1, 0);
				config.ReportApiVersions = true;
				config.ReportApiVersions = true;
			});

			services.AddVersionedApiExplorer(delegate (ApiExplorerOptions options)
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
			});

			services.AddSwaggerGen();

			services.AddCustomHealthCheck(configuration);
        }
    }
}
