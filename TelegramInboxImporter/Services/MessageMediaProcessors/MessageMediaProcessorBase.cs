using TelegramInboxImporter.Clients;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public abstract class MessageMediaProcessorBase(ITelegramClient client)
{
    protected ITelegramClient _client = client;
}
