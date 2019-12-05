using Ninject.Modules;
using TaskManager.Bot.Telegram;
using TaskManager.Bot.Telegram.Commands;

namespace TaskManager.Ioc.Modules
{
    public class CommandModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommand>().To<AuthorizationCommand>();
            Bind<ICommand>().To<GetInactiveTaskList>();
            Bind<ICommand>().To<GetInactiveTaskList>();
            Bind<ICommand>().To<GetInactiveTaskList>();
            Bind<ICommand>().To<AddTask>();
            Bind<ICommand>().To<GetTaskInfo>();
            Bind<ICommand>().ToConstant(new StubCommand("Статистика по задачам (в разработке)"));
            Bind<ICommand>().ToConstant(new StubCommand("help (в разработке)"));
        }
    }
}