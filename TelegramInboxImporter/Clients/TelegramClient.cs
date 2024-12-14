using TelegramInboxImporter.Common;
using TL;
using WTelegram;

namespace TelegramInboxImporter.Clients;

public class TelegramClient : ITelegramClient
{

    /*
        https://wiz0u.github.io/WTelegramClient/EXAMPLES

        1. Login into Telegram. There was some note it could be done easier in interactive mode. To check remove current session, implement
           that interactive approach and try
        2. Add serilog logging and re-route Telegram logging into it
        3. What to do with Telegram Media instances? Save as binaries?
        4. How to import into Obsidian? Each message convert into MD-file with a cetain markdown?
        5. How to track imported messages? Persist offsetId and next time try to start from it
    */

    private readonly Client _client;

    // public delegate void ProgressCallback(long transmitted, long totalSize);

    public TelegramClient()
    {
        _client = new Client(Config);
    }

    public async Task<Storage_FileType> DownloadFileAsync(Photo photo, Stream outputStream, PhotoSizeBase? photoSize = null, Client.ProgressCallback? progress = null) =>
        await _client.DownloadFileAsync(photo, outputStream, photoSize, progress);

    public async Task<string> DownloadFileAsync(Document document, Stream outputStream, PhotoSizeBase? thumbSize = null, Client.ProgressCallback? progress = null) =>
        await _client.DownloadFileAsync(document, outputStream, thumbSize, progress);

    public async Task<IEnumerable<Message>> GetMessagesHistoryAsync(string chatName, int minId = 0)
    {
        if (string.IsNullOrWhiteSpace(chatName))
        {
            throw new ArgumentNullException(nameof(chatName), "Argument cannot be null or empty");
        }

        var user = await _client.LoginUserIfNeeded();

        Console.WriteLine($"Logged in as {user.username ?? $"{user.first_name} {user.last_name}"}");

        var allChats = await _client.Messages_GetAllChats();
        InputPeer chatPeer = allChats.chats
            .FirstOrDefault(kvp =>
                kvp.Value.Title.StartsWith(chatName, StringComparison.CurrentCultureIgnoreCase) &&
                kvp.Value.IsActive
            )
            .Value ?? throw new InvalidOperationException($"'{chatName}' chat doesn't exist");

        var history = await _client.Messages_GetHistory(peer: chatPeer, min_id: minId);

        var result = new List<Message>();
        foreach (var msgBase in history.Messages)
        {
            if (msgBase is Message msg)
            {
                result.Add(msg);
            }
        }
        return result;

    }

    private static string? Config(string what)
    {
        return what switch
        {
            "api_id" => "",
            "api_hash" => "",
            "phone_number" => "",
            "password" => CredentialManager.GetTelegramPassword(),
            _ => null,
        };
    }
}
