using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NCQ.Core.Application
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCoreApplication(this IServiceCollection services, IConfiguration configuration)
        {
            #region MediatR Registration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            #endregion MediatR Registration
        }
    }
}
