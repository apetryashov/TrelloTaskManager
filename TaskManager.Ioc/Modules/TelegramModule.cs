using MihaZupan;
using Ninject.Modules;
using TaskManager.Bot.Telegram;
using TaskManaget.Bot;
using Telegram.Bot;

namespace TaskManager.Ioc.Modules
{
    public class TelegramModule : NinjectModule
    {
        private readonly string accessToken;

        public TelegramModule(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public override void Load()
        {
            var proxy = new HttpToSocks5Proxy(
                "proxy-host", 999, "username", "pwd"
            );
            proxy.ResolveHostnamesLocally = true;
            Bind<ITelegramBotClient>().ToConstant(new TelegramBotClient(accessToken, proxy));
            Bind<IBot>().To<TgBot>();
        }
    }
}