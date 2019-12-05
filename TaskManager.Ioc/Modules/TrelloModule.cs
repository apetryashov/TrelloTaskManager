using Manatee.Trello;
using Ninject.Modules;
using TaskManager.Common;
using TaskManager.Common.Tasks;
using TaskManager.Trello;

namespace TaskManager.Ioc.Modules
{
    public class TrelloModule : NinjectModule
    {
        private readonly string appKey;

        public TrelloModule(string appKey)
        {
            this.appKey = appKey;
        }

        public override void Load()
        {
            Bind<AppKey>().ToConstant(new AppKey {Key = appKey});
            Bind<ITrelloFactory>().To<TrelloFactory>();
            Bind<ITaskHandler>().To<TrelloTasksHandler>();
            Bind<IAuthorizationProvider>().To<TrelloAuthorizationProvider>();
        }
    }
}