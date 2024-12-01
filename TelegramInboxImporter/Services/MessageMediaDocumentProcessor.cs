using TL;
using WTelegram;

namespace TelegramInboxImporter.Services;

public class MessageMediaDocumentProcessor(
    Client client,
    MessageMediaDocument messageMedia) : MessageMediaProcessorBase(client), IMessageMediaProcessor
{
    private MessageMediaDocument _messageMedia = messageMedia;

    public Task ProcessAsync(string markdownContent)
    {
        throw new NotImplementedException();
    }
}
