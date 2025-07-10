using Microsoft.Extensions.DependencyInjection;

namespace NCQ.Core.Middleware
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreMiddleware(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }
    }
}
