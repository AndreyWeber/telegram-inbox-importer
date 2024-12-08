using TL;

namespace TelegramInboxImporter.Clients;

public interface ITelegramClient
{
    Task<IEnumerable<Message>> GetMessagesHistoryAsync(string chatName, int minId = 0);
}
