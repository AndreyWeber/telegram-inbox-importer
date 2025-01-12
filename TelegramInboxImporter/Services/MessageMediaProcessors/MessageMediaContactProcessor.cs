using TelegramInboxImporter.Clients;
using TL;
using WTelegram;

namespace TelegramInboxImporter.Services.MessageMediaProcessors;

public class MessageMediaContactProcessor(
    ITelegramClient client,
    MessageMediaContact messageMedia,
    MessagesProcessorSettings settings
) : MessageMediaProcessorBase(client, settings), IMessageMediaProcessor
{
    private MessageMediaContact _messageMedia = messageMedia;

    public async Task<string> ProcessAsync(string markdownContent)
    {
        // // Extract contact details
        // string phoneNumber = contact.phone_number;
        // string firstName = contact.first_name;
        // string lastName = contact.last_name ?? string.Empty;

        // // Display or log the contact information
        // Console.WriteLine($"Contact Name: {firstName} {lastName}");
        // Console.WriteLine($"Phone Number: {phoneNumber}");

        // // Optionally, save the contact information to a file
        // string contactInfo = $"Name: {firstName} {lastName}\nPhone: {phoneNumber}\n";
        // string fileName = $"{firstName}_{lastName}_contact.txt";
        // await File.WriteAllTextAsync(fileName, contactInfo);
        // Console.WriteLine($"Contact information saved to: {fileName}");

        return string.Empty;
    }
}
