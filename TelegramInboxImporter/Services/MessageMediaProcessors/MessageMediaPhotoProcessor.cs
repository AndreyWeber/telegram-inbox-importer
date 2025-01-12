using TelegramInboxImporter.Clients;
using TL;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public class MessageMediaPhotoProcessor : MessageMediaProcessorBase, IMessageMediaProcessor
{
    private readonly Photo _photo;

    public MessageMediaPhotoProcessor(
        ITelegramClient client,
        MessageMediaPhoto messageMedia,
        MessagesProcessorSettings settings) : base(client, settings)
    {
        if (messageMedia.photo is not Photo photo)
        {
            throw new InvalidOperationException($"{nameof(messageMedia)} doesn't contain photo of {typeof(Photo)} type");
        }

        _photo = photo;
    }

    public async Task<string> ProcessAsync(string markdownContent)
    {
        if (markdownContent == null)
        {
            throw new ArgumentNullException(nameof(markdownContent), "Argument cannot be null");
        }

        // Prepare output image file path
        var fileSavePath = Path.Combine(_settings.VaultPath, _settings.FilesFolder);
        var photoIdFileName = Path.Combine(fileSavePath, $"{_photo.id}.jpg");

        // Download output file from the Telegram MessageMedia
        using var fileStream = File.Create(photoIdFileName);
        var fileType = await _client.DownloadFileAsync(_photo, fileStream);
        fileStream.Close();

        // Rename file with name taken from the MessageMedia
        var messageMediaFileName = string.Empty;
        if (fileType is not Storage_FileType.unknown and not Storage_FileType.partial)
        {
            messageMediaFileName = Path.Combine(fileSavePath, $"{_photo.id}.{fileType}");
            File.Move(photoIdFileName, messageMediaFileName, true);
        }

        // Modify markdown content by adding output file path
        var modifiedMarkdownContent = $"{markdownContent}\r\n[[" +
            $"{(string.IsNullOrWhiteSpace(messageMediaFileName) ? photoIdFileName : messageMediaFileName)}]]";

        return modifiedMarkdownContent;
    }
}
