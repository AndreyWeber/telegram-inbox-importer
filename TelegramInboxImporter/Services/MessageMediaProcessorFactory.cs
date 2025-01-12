using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Services.MessageMediaProcessors;
using TL;

namespace TelegramInboxImporter.Services;

public static class MessageMediaProcessorFactory
{
    public static IMessageMediaProcessor GetMediaProcessor(ITelegramClient client, MessageMedia messageMedia)
    {
        return messageMedia switch
        {
            MessageMediaDocument messageMediaDocument => new MessageMediaDocumentProcessor(
                client, messageMediaDocument),
            MessageMediaPhoto messageMediaPhoto => new MessageMediaPhotoProcessor(
                client, messageMediaPhoto),
            MessageMediaWebPage messageMediaWebPage => new MessageMediaWebPageProcessor(
                client, messageMediaWebPage),
            MessageMediaContact messageMediaContact => new MessageMediaContactProcessor(
                client, messageMediaContact),
            null => throw new ArgumentNullException(nameof(messageMedia), "Argument cannot be null"),
            _ => throw new InvalidOperationException($"Unknown MessageMedia type: {messageMedia.GetType()}")
        };
    }
}
