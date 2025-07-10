using NCQ.Core.Application;
using NCQ.Infrastructure.Repositories;
using NCQ.Infrastructure.Shared;
using NCQ.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseDefaultServiceProvider((_, options) => options.ValidateScopes = true);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;
var hostEnvironment = builder.Environment;

services.AddHttpContextAccessor();
services.AddCoreMiddleware();
services.AddControllers();
services.AddRegisterSharedServices(configuration);

services.AddRegisterRepositories(configuration, hostEnvironment);
services.AddCoreApplication(configuration);

try
{
	var app = builder.Build();
	var provider = services.BuildServiceProvider();

	app.UseSharedServices(provider);
	app.UseCors("AllowAll");
	app.UseAuthorization();

	app.MapControllers();
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			options.RoutePrefix = "todo/swagger";
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
		});
	}
	app.Run();
}
catch (Exception ex)
{
	Console.WriteLine("❌ Application startup failed:");
	Console.WriteLine(ex.ToString());

	if (ex is AggregateException aggEx)
	{
		foreach (var inner in aggEx.InnerExceptions)
		{
			Console.WriteLine("➡ Inner Exception: " + inner.GetType().Name);
			Console.WriteLine(inner.Message);
			Console.WriteLine(inner.StackTrace);
		}
	}
}

