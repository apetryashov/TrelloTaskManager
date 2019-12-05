using LiteDB;
using Ninject.Modules;
using TaskManager.Bot.Telegram.Commands;

namespace TaskManager.Ioc.Modules
{
    public class LiteDbStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<LiteDatabase>().ToConstant(new LiteDatabase("MyDb"));
            Bind<IAuthorizationStorage>().To<LiteDbAuthorizationStorage>().InSingletonScope();
        }
    }
}