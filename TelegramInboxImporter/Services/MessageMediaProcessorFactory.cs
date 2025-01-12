using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Services.MessageMediaProcessors;
using TL;

namespace TelegramInboxImporter.Services;

public static class MessageMediaProcessorFactory
{
    public static IMessageMediaProcessor GetMediaProcessor(
        ITelegramClient client,
        MessageMedia messageMedia,
        MessagesProcessorSettings settings
    )
    {
        return messageMedia switch
        {
            MessageMediaDocument messageMediaDocument => new MessageMediaDocumentProcessor(
                client, messageMediaDocument, settings),
            MessageMediaPhoto messageMediaPhoto => new MessageMediaPhotoProcessor(
                client, messageMediaPhoto, settings),
            MessageMediaWebPage messageMediaWebPage => new MessageMediaWebPageProcessor(
                client, messageMediaWebPage, settings),
            MessageMediaContact messageMediaContact => new MessageMediaContactProcessor(
                client, messageMediaContact, settings),
            null => throw new ArgumentNullException(nameof(messageMedia), "Argument cannot be null"),
            _ => throw new InvalidOperationException($"Unknown MessageMedia type: {messageMedia.GetType()}")
        };
    }
}
