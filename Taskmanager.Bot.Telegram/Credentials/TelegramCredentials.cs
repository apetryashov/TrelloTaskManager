using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Bot.Telegram.Credentials
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class TelegramCredentials
    {
        public string AccessToken { get; set; }
        public string AppKey { get; set; }
    }
}