using Ninject.Modules;
using TaskManager.Bot.Telegram;
using TaskManager.Common.Tasks;
using TaskManaget.Bot;
using TaskManaget.Bot.Commands;

namespace TaskManager.Ioc.Modules
{
    public class CommandModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommand>().To<AuthorizationCommand>();
            Bind<ICommand>().To<GetTaskTaskList>().WithConstructorArgument("taskStatus", TaskStatus.Inactive);
            Bind<ICommand>().To<GetTaskTaskList>().WithConstructorArgument("taskStatus", TaskStatus.Active);
            Bind<ICommand>().To<GetTaskTaskList>().WithConstructorArgument("taskStatus", TaskStatus.Resolved);
            Bind<ICommand>().To<AddTask>();
            Bind<ICommand>().To<GetTaskInfo>();
            Bind<ICommand>().To<ChangeTaskStatusCommand>();
            Bind<ICommand>().ToConstant(new StubCommand("Статистика по задачам (в разработке)"));
            Bind<ICommand>().ToConstant(new StubCommand("help (в разработке)"));
        }
    }
}