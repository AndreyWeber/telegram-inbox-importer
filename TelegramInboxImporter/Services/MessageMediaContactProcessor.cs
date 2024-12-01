using TL;
using WTelegram;

namespace TelegramInboxImporter.Services;

public class MessageMediaContactProcessor(
    Client client,
    MessageMediaContact messageMedia) : MessageMediaProcessorBase(client), IMessageMediaProcessor
{
    private MessageMediaContact _messageMedia = messageMedia;

    public Task ProcessAsync(string markdownContent)
    {
        throw new NotImplementedException();
    }
}
