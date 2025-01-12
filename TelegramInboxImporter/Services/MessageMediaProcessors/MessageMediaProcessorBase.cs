using TelegramInboxImporter.Clients;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public abstract class MessageMediaProcessorBase(ITelegramClient client, MessagesProcessorSettings settings)
{
    protected ITelegramClient _client = client;
    protected MessagesProcessorSettings _settings = settings;
}
