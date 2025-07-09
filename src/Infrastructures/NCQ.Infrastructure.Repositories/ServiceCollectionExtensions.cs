using DbContext.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCQ.Infrastructure.Repositories.Configuration;
using NCQ.Infrastructure.Repositories.Tasks.Repository;


namespace NCQ.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var connectionConfigs = new ConnectionConfigs();
            configuration.Bind(nameof(ConnectionConfigs), connectionConfigs);
            services.AddSingleton(connectionConfigs);

            services.AddSingleton<IConnection>(_ => new Connection(configuration));
            #region Task
            services.AddScoped<ITaskRepository, TaskRepository>();
            #endregion
        }
    }
}
