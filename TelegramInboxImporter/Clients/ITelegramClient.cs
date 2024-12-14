using TL;
using WTelegram;

namespace TelegramInboxImporter.Clients;

public interface ITelegramClient
{
    Task<IEnumerable<Message>> GetMessagesHistoryAsync(string chatName, int minId = 0);
    Task<Storage_FileType> DownloadFileAsync(Photo photo, Stream outputStream, PhotoSizeBase? photoSize = null, Client.ProgressCallback? progress = null);
    Task<string> DownloadFileAsync(Document document, Stream outputStream, PhotoSizeBase? thumbSize = null, Client.ProgressCallback? progress = null);
}
