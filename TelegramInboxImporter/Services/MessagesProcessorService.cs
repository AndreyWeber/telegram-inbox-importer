using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelegramInboxImporter.Clients;
using TelegramInboxImporter.Common;
using TelegramInboxImporter.Services.MessageMediaProcessors;
using TL;

namespace TelegramInboxImporter.Services;

public class MessagesProcessorService : IMessagesProcessorService
{
    private readonly ILogger<MessagesProcessorService> _logger;
    private readonly ITelegramClient _telegramClient;
    private readonly Func<ITelegramClient, MessageMedia, IMessageMediaProcessor> _messageMediaProcessorFactory;
    private readonly MessagesProcessorSettings _settings;

    public MessagesProcessorService(
        ILogger<MessagesProcessorService> logger,
        IOptions<MessagesProcessorSettings> options,
        ITelegramClient telegramClient,
        Func<ITelegramClient, MessageMedia, IMessageMediaProcessor> messageMediaProcessorFactory)
    {
        _logger = logger;
        _telegramClient = telegramClient;
        _messageMediaProcessorFactory = messageMediaProcessorFactory;
        _settings = options.Value;
    }

    public async Task ProcessAsync()
    {
        // TODO: Read minId from persistent storage
        var minId = IMessagesProcessorService.DefaultMinId;

        IEnumerable<Message> messages = [];
        try
        {
            messages = await _telegramClient.GetMessagesHistoryAsync(_settings.TelegramChatName, minId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get messages from Telegram chat {chatName}", _settings.TelegramChatName);
        }

        foreach (var message in messages)
        {
            // TODO: Create markdown content
            // var from = history.UserOrChat(msgBase.From ?? msgBase.Peer);
            var messageDate = message.Date;
            var messageText = message.message;
            var messageMedia = message.media;

            if (messageMedia is null)
            {
                // TODO: Save markdown content and continue, because there is no media
                continue;
            }

            try
            {
                var processor = _messageMediaProcessorFactory(_telegramClient, messageMedia);
                // var processor = messageMedia.GetMediaProcessor(_telegramClient);
                // TODO: Make ProcessAsync to return modified markdown content
                await processor.ProcessAsync(messageText);
                // TODO: Save modified markdown content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get a MediaProcessor for the Telegram chat '{chatName}'", _settings.TelegramChatName);
            }
        }

        // TODO: Save minId into persistent storage
        var offestId = messages.FirstOrDefault<Message>()?.ID
            ?? IMessagesProcessorService.DefaultMinId;
    }

    private async Task SaveMessageMarkdown(string messageMarkdown)
    {
        if (messageMarkdown == null)
        {
            throw new ArgumentNullException(nameof(messageMarkdown), "Argument cannot be null");
        }
    }
}