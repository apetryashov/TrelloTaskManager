using Ninject.Modules;
using TaskManager.Bot.Telegram;
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
            Bind<ITelegramBotClient>().ToConstant(new TelegramBotClient(accessToken));
            Bind<IBot>().To<TgBot>();
        }
    }
}