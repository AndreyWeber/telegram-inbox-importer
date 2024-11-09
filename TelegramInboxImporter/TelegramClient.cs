using TL;
using WTelegram;

namespace TelegramInboxImporter;

public class TelegramClient
{

    /*
        https://wiz0u.github.io/WTelegramClient/EXAMPLES

        1. Login into Telegram. There was some note it could be doen easier in interactive mode. To check remove current session, implement
           that interactive approach and try
        2. Add serilog logging and re-route Telegram logging into it
        3. What to do with Telegram Media instances? Save as binaries?
        4. How to import into Obsidian? Each message convert into MD-file with a cetain markdown?
        5. How to track imported messages? Persist offsetId and next time try to start from it
    */

    private Client _client;

    public TelegramClient()
    {
        _client = new Client(Config);
    }

    public async Task PrintMessages()
    {
        var user = await _client.LoginUserIfNeeded();

        Console.WriteLine($"Logged in as {user.username ?? ($"{user.first_name} {user.last_name}")}");

        var allChats = await _client.Messages_GetAllChats();
        InputPeer inboxChatPeer = allChats.chats
            .FirstOrDefault(kvp =>
                kvp.Value.Title.StartsWith("inbox", StringComparison.CurrentCultureIgnoreCase) &&
                kvp.Value.IsActive
            ).Value;

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
                    Console.WriteLine($"{from}> {msg.message} {msg.media}");
                }
                else if (msgBase is MessageService ms)
                {
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
