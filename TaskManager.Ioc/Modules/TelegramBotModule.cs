using Microsoft.Extensions.DependencyInjection;
using MihaZupan;
using TaskManager.Bot;
using Telegram.Bot;

namespace TaskManager.Ioc.Modules
{
    public class TelegramBotModule : IServiceModule
    {
        private readonly BotConfiguration config;

        public TelegramBotModule(BotConfiguration botConfiguration) => config = botConfiguration;

        public void Load(IServiceCollection serviceCollection)
        {
            var client = string.IsNullOrEmpty(config.Socks5Host)
                ? new TelegramBotClient(config.BotToken)
                : new TelegramBotClient(
                    config.BotToken,
                    new HttpToSocks5Proxy(config.Socks5Host, config.Socks5Port));

            serviceCollection.AddSingleton(client);
        }
    }
}