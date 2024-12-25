namespace TelegramInboxImporter.Services;

public interface IMessagesProcessorService
{
    static int DefaultMinId => 0;
    static string TelegramChatNameConfigNode => "TelegramChatName";

    Task ProcessAsync();
}
