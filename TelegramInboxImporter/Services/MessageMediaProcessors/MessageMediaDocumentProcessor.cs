using TelegramInboxImporter.Clients;
using TL;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public class MessageMediaDocumentProcessor : MessageMediaProcessorBase, IMessageMediaProcessor
{
    private readonly Document _document;

    public MessageMediaDocumentProcessor(
        ITelegramClient client,
        MessageMediaDocument messageMedia,
        MessagesProcessorSettings settings
    ) : base(client, settings)
    {
        if (messageMedia.document is not Document document)
        {
            throw new InvalidOperationException($"{nameof(messageMedia)} doesn't contain document of {typeof(Document)} type");
        }

        _document = document;
    }

    public async Task<string> ProcessAsync(string markdownContent)
    {
        if (markdownContent == null)
        {
            throw new ArgumentNullException(nameof(markdownContent), "Argument cannot be null");
        }

        var documentName = _document.Filename
            ?? $"{_document.id}.{_document.mime_type[(_document.mime_type.IndexOf('/') + 1)..]}";

        var fileName = Path.Combine(
            @"C:\Users\andre\OneDrive\Documents\Obsidian Vault\files",
            documentName);

        using var fileStream = File.Create(fileName);
        await _client.DownloadFileAsync(_document, fileStream);

        return string.Empty;
    }
}
