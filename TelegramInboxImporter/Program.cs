// See https://aka.ms/new-console-template for more information
using System.Text;
using TelegramInboxImporter;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Hello, World!");

var telegramClient = new TelegramClient();

await telegramClient.PrintMessages();

