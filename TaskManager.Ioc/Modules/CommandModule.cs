using Ninject.Modules;
using TaskManager.Bot;
using TaskManager.Bot.Commands;
using TaskManager.Common.Tasks;

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
            Bind<ICommand>().ToConstant(new TextCommand("Road map", @"
В скором времени, в боте появится новый функционал:
1. Напоминание о текущих задачах (с целью актуализации состояния)
2. Статистика по задачам
3. ...
"));
        }
    }
}