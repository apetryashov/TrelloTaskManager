using System.Diagnostics.CodeAnalysis;

namespace Bot.Telegram.Common.Credentials
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class TelegramCredentials
    {
        public string AccessToken { get; set; }
        public string AppKey { get; set; }
    }
}