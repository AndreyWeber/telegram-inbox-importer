using TL;
using WTelegram;

namespace TelegramInboxImporter.Services;

public abstract class MessageMediaProcessorBase(Client client)
{
    protected Client _client = client;
}
