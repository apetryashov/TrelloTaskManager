using Bot.Telegram.Common;
using Bot.Telegram.Common.Credentials;

namespace Bot.Telegram.ConsoleClient
{
    static class EntryPoint
    {
        private static void Main()
        {
            var credentials = new TelegramCredentials
            {
                AccessToken = "access_token"
            };
            new TgBot(credentials).Start();
        }
    }
}