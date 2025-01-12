using TelegramInboxImporter.Clients;
using TL;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public class MessageMediaWebPageProcessor(
    ITelegramClient client,
    MessageMediaWebPage messageMedia,
    MessagesProcessorSettings settings
) : MessageMediaProcessorBase(client, settings), IMessageMediaProcessor
{
    private MessageMediaWebPage _messageMedia = messageMedia;

    public async Task<string> ProcessAsync(string markdownContent)
    {
        // // Extract web page details
        // string title = webPage.title ?? "No Title";
        // string description = webPage.description ?? "No Description";
        // string url = webPage.url ?? "No URL";

        // // Display or log the web page information
        // Console.WriteLine($"Web Page Title: {title}");
        // Console.WriteLine($"Description: {description}");
        // Console.WriteLine($"URL: {url}");

        // // If the web page contains a photo, download it
        // if (webPage.photo is Photo photo)
        // {
        //     string photoFilePath = await SavePhotoAsync(client, photo);
        //     Console.WriteLine($"Downloaded web page photo to: {photoFilePath}");
        // }

        return string.Empty;
    }
}
