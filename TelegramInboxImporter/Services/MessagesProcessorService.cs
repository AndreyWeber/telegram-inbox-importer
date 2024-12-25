using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Common;
using TL;
using WTelegram;

namespace TelegramInboxImporter.Services;

public class MessagesProcessorService : IMessagesProcessorService
{
    private readonly ILogger<MessagesProcessorService> _logger;
    private readonly ITelegramClient _telegramClient;
    private readonly string _chatName;

    public MessagesProcessorService(
        ILogger<MessagesProcessorService> logger,
        IConfiguration configuration,
        ITelegramClient telegramClient)
    {
        _logger = logger;
        _telegramClient = telegramClient;
        _chatName = configuration.GetValue<string>(IMessagesProcessorService.TelegramChatNameConfigNode)
            ?? throw new ConfigurationErrorsException(
                $"Invalid value for '{IMessagesProcessorService.TelegramChatNameConfigNode}' config node");
    }

    public async Task ProcessAsync()
    {
        // TODO: Read minId from persistent storage
        var minId = IMessagesProcessorService.DefaultMinId;

        IEnumerable<Message> messages = [];
        try
        {
            messages = await _telegramClient.GetMessagesHistoryAsync(_chatName, minId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get messages from Telegram chat {chatName}", _chatName);
        }

        foreach (var message in messages)
        {
            // TODO: Create markdown content
            // var from = history.UserOrChat(msgBase.From ?? msgBase.Peer);
            var messageText = message.message;
            var messageMedia = message.media;

            if (messageMedia is null)
            {
                // TODO: Save markdown content and continue, because there is no media
                continue;
            }

            try
            {
                var processor = messageMedia.GetMediaProcessor(_telegramClient);
                // TODO: Make ProcessAsync to return modified markdown content
                await processor.ProcessAsync(messageText);
                // TODO: Save modified markdown content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get a MediaProcessor for the Telegram chat '{chatName}'", _chatName);
            }
        }

        // TODO: Save minId into persistent storage
        var offestId = messages.FirstOrDefault<Message>()?.ID
            ?? IMessagesProcessorService.DefaultMinId;
    }

}