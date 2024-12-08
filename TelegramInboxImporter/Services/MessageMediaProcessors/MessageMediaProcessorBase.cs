using TL;
using WTelegram;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public abstract class MessageMediaProcessorBase(Client client)
{
    protected Client _client = client;
}
