using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Services;
using TelegramInboxImporter.Services.MessageMediaProcessors;

namespace TelegramInboxImporter;

public class SomeSettings
{
    public required string Test { get; set; }
}

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigurationServices(IServiceCollection services)
    {
        // services.Configure<SomeSettings>(Configuration.GetSection("SomeSettings"));
        services.AddSingleton<ITelegramClient, TelegramClient>();
        services.AddSingleton<IMessagesProcessorService, MessagesProcessorService>();
        services.AddHostedService<HostedMessagesProcessorService>();
    }
}
