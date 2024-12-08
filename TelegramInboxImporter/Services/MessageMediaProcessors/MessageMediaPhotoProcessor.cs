using TL;
using WTelegram;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public class MessageMediaPhotoProcessor : MessageMediaProcessorBase, IMessageMediaProcessor
{
    private readonly Photo _photo;

    public MessageMediaPhotoProcessor(Client client, MessageMediaPhoto messageMedia) : base(client)
    {
        if (messageMedia.photo is not Photo photo)
        {
            throw new InvalidOperationException($"{nameof(messageMedia)} doesn't contain photo of {typeof(Photo)} type");
        }

        _photo = photo;
    }

    public async Task ProcessAsync(string markdownContent)
    {
        if (markdownContent == null)
        {
            throw new ArgumentNullException(nameof(markdownContent), "Argument cannot be null");
        }

        const string savePath = @"C:\Users\andre\OneDrive\Documents\Obsidian Vault\files";

        var fileName = Path.Combine(savePath, $"{_photo.id}.jpg");

        using var fileStream = File.Create(fileName);
        var fileType = await _client.DownloadFileAsync(_photo, fileStream);
        fileStream.Close();

        if (fileType is not Storage_FileType.unknown and not Storage_FileType.partial)
        {
            var newFileName = Path.Combine(savePath, $"{_photo.id}.{fileType}");
            File.Move(fileName, newFileName, true);
        }
    }
}
