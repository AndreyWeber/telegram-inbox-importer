namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public interface IMessageMediaProcessor
{
    Task<string> ProcessAsync(string markdownContent);
}
