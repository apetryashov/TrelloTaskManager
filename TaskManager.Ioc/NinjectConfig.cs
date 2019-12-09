using Ninject;
using TaskManager.Bot;
using TaskManager.Bot.Telegram.Credentials;
using TaskManager.Ioc.Modules;

namespace TaskManager.Ioc
{
    public class NinjectConfig
    {
        public static IKernel GetKernel(TelegramCredentials credentials)
        {
            var kernel = new StandardKernel(
                new CommandModule(),
                new TelegramModule(credentials.AccessToken),
                new TrelloModule(credentials.AppKey),
                new LiteDbStorageModule());

            kernel.Bind<IRequestHandler>().To<RequestHandler>();

            return kernel;
        }
    }
}