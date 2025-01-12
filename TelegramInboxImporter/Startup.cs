using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Common;
using TelegramInboxImporter.Services;
using TelegramInboxImporter.Services.MessageMediaProcessors;
using TL;

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
        services.Configure<MessagesProcessorSettings>(Configuration.GetSection("MessagesProcessorSettings"));

        services.AddTransient<Func<ITelegramClient, MessageMedia, IMessageMediaProcessor>>(
            serviceProvider => (telegramClient, messageMedia) =>
                MessageMediaProcessorFactory.GetMediaProcessor(telegramClient, messageMedia)
        );

        services.AddSingleton<ITelegramClient, TelegramClient>();
        services.AddSingleton<IMessagesProcessorService, MessagesProcessorService>();
        services.AddHostedService<HostedMessagesProcessorService>();
    }
}
