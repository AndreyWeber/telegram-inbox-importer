namespace TelegramInboxImporter.Services;

public interface IMessageMediaProcessor
{
    Task ProcessAsync(string markdownContent);
}
