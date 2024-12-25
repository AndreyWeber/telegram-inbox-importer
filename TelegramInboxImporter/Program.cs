using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TelegramInboxImporter;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(Serilog.Events.LogEventLevel.Verbose)
    .CreateBootstrapLogger();
try
{
    Log.Information("Starting up...");

    var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

    Log.Information($"DOTNET_ENVIRONMENT={environment ?? "<NotSet>"}");

#if DEBUG
    environment ??= "Development";
#else
    environment ??= "Production";
#endif

    var host = Host.CreateDefaultBuilder(args)
        .UseEnvironment(environment)
        .ConfigureLogging(logging => {
            logging.ClearProviders();
            logging.AddSerilog();
        })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true
            );
        })
        .ConfigureServices((context, services) =>
        {
            var startup = new Startup(context.Configuration);
            startup.ConfigurationServices(services);
        })
        .UseConsoleLifetime(options => options.SuppressStatusMessages = true)
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unexpected application error occurred");
}
finally
{
    Log.Information("Application stopped");
    Log.CloseAndFlush();
}
