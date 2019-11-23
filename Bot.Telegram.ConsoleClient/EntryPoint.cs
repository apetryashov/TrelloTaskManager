using System.IO;
using Bot.Telegram.Common;
using Bot.Telegram.Common.Credentials;
using Newtonsoft.Json;

namespace Bot.Telegram.ConsoleClient
{
    static class EntryPoint
    {
        private static void Main()
        {
            var telegramCredentials = JsonConvert.DeserializeObject<TelegramCredentials>(File.ReadAllText("Credentials/TelegramCredentials.json"));
            new TgBot(telegramCredentials).Start();
        }
    }
}