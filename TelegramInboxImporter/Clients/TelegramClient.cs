using TelegramInboxImporter.Common;
using TL;
using WTelegram;

namespace TelegramInboxImporter.Clients;

public class TelegramClient
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

    private const string inboxChat = "Inbox";

    private Client _client;

    public TelegramClient()
    {
        _client = new Client(Config);
    }

    public async Task PrintMessages()
    {
        var user = await _client.LoginUserIfNeeded();

        Console.WriteLine($"Logged in as {user.username ?? $"{user.first_name} {user.last_name}"}");

        var allChats = await _client.Messages_GetAllChats();
        InputPeer inboxChatPeer = allChats.chats
            .FirstOrDefault(kvp =>
                kvp.Value.Title.StartsWith(inboxChat, StringComparison.CurrentCultureIgnoreCase) &&
                kvp.Value.IsActive
            )
            .Value ?? throw new InvalidOperationException($"'{inboxChat}' chat doesn't exist");

        for (var offsetId = 0; ;)
        {
            var messages = await _client.Messages_GetHistory(peer: inboxChatPeer, offset_id: offsetId);
            if (messages.Messages.Length == 0)
            {
                break;
            }

            foreach (var msgBase in messages.Messages)
            {
                var from = messages.UserOrChat(msgBase.From ?? msgBase.Peer);
                if (msgBase is Message msg)
                {
                    Console.WriteLine($"{from}> {msg.message} :: {msg.media}");

                    // TL.MessageMediaWebPage
                    // TL.MessageMediaPhoto
                    if (msg.media is null)
                    {
                        continue;
                    }

                    try
                    {
                        var proc = msg.media.GetMediaProcessor(_client);
                        await proc.ProcessAsync(msg.message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (msgBase is MessageService ms)
                {
                    // TODO: Log service message
                    Console.WriteLine($"{from}> [{ms.action.GetType().Name[13..]}]");
                }
            }
            offsetId = messages.Messages[^1].ID;
        }
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
