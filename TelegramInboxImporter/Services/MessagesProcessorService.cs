using Microsoft.Extensions.Logging;
using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Common;
using TL;

namespace TelegramInboxImporter.Services;

public class MessagesProcessorService(ILogger<MessagesProcessorService> logger, ITelegramClient telegramClient) : IMessagesProcessorService
{
    private readonly ILogger<MessagesProcessorService> _logger = logger;
    private readonly ITelegramClient _telegramClient = telegramClient;

    public async Task ProcessAsync()
    {
        // read minId from persistent storage

        const string chatName = "Inbox";

        IEnumerable<Message> messages = [];
        try
        {
            messages = await _telegramClient.GetMessagesHistoryAsync(chatName, minId: 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get messages from Telegram chat {0}", chatName);
        }

        foreach (var message in messages)
        {
            // Create markdown content
            // var from = history.UserOrChat(msgBase.From ?? msgBase.Peer);
            var messageText = message.message;

            if (message.media is null)
            {
                // Save markdown content
                continue;
            }

            try
            {
                // var proc = message.media.GetMediaProcessor(_telegramClient);
                // await proc.ProcessAsync(messageText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // else if (msgBase is MessageService ms)
            // {
            //     // TODO: Log service message
            //     Console.WriteLine($"{from}> [{ms.action.GetType().Name[13..]}]");
            // }
            // offsetId = messages.Messages[^1].ID;
        }
        var offestId = messages.First().ID;
    }

}