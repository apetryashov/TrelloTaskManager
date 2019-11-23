using System;
using System.Threading;
using Bot.Telegram.Common.Credentials;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Bot.Telegram.Common
{
    public class TgBot
    {
        private readonly ITelegramBotClient botClient;

        public TgBot(TelegramCredentials credentials)
        {
            botClient = new TelegramBotClient(credentials.AccessToken);
        }

        public void Start()
        {
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null)
                return;

            Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

            await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "You said:\n" + e.Message.Text
            );
        }
    }
}