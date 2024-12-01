using TelegramInboxImporter.Services;
using TL;
using WTelegram;

namespace TelegramInboxImporter.Common;

public static class MessageMediaExtensions
{
    public static IMessageMediaProcessor GetMediaProcessor(this MessageMedia messageMedia, Client client)
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