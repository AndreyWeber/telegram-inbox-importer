using TL;
using WTelegram;

namespace TelegramInboxImporter.Services;

public class MessageMediaWebPageProcessor(
    Client client,
    MessageMediaWebPage messageMedia) : MessageMediaProcessorBase(client), IMessageMediaProcessor
{
    private MessageMediaWebPage _messageMedia = messageMedia;

    public Task ProcessAsync(string markdownContent)
    {
        throw new NotImplementedException();
    }
}
