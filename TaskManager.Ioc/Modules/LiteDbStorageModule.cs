using LiteDB;
using Ninject.Modules;
using TaskManaget.Bot.Authorization;

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