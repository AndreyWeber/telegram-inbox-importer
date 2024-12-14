using Microsoft.Extensions.Logging;
using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Common;
using TL;
using WTelegram;

namespace TelegramInboxImporter.Services;

public class MessagesProcessorService(ILogger<MessagesProcessorService> logger, ITelegramClient telegramClient) : IMessagesProcessorService
{
    private const int DefaultMinId = 0;

    private readonly ILogger<MessagesProcessorService> _logger = logger;
    private readonly ITelegramClient _telegramClient = telegramClient;

    public async Task ProcessAsync()
    {
        // Read minId from persistent storage
        var minId = DefaultMinId;

        const string chatName = "Inbox";

        IEnumerable<Message> messages = [];
        try
        {
            messages = await _telegramClient.GetMessagesHistoryAsync(chatName, minId);
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
            var messageMedia = message.media;

            if (messageMedia is null)
            {
                // Save markdown content
                continue;
            }

            try
            {
                var proc = messageMedia.GetMediaProcessor(_telegramClient);
                // await proc.ProcessAsync(messageText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // else if (msgBase is MessageService ms)
            // {
            //     Console.WriteLine($"{from}> [{ms.action.GetType().Name[13..]}]");
            // }
            // offsetId = messages.Messages[^1].ID;
        }

        // Save minId into persistent storage
        var offestId = messages.FirstOrDefault<Message>()?.ID ?? DefaultMinId;
    }

}