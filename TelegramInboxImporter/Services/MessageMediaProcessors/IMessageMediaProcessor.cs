namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public interface IMessageMediaProcessor
{
    Task ProcessAsync(string markdownContent);
}
