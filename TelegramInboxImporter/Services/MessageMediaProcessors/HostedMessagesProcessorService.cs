using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public class HostedMessagesProcessorService(
    ILogger<HostedMessagesProcessorService> logger,
    IHostApplicationLifetime lifetime,
    IMessagesProcessorService messagesProcessorService) : IHostedService
{
    private readonly ILogger<HostedMessagesProcessorService> _logger = logger;
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IMessagesProcessorService _messagesProcessorService = messagesProcessorService;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Pass cancellation token. Add handling of cacellation token inside ProcessAsync()
            await _messagesProcessorService.ProcessAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred: {exMsg}", ex.Message);
        }

        _lifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
