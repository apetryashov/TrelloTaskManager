using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Bot;
using TaskManager.Bot.Commands;
using TaskManager.Common.Tasks;

namespace TaskManager.Ioc.Modules
{
    public class CommandModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<ICommand, AuthorizationCommand>();

            var taskStatuses = Enum
                .GetValues(typeof(TaskStatus))
                .Cast<TaskStatus>()
                .ToList();

            taskStatuses
                .ForEach(AddGetTaskTaskListCommand);

            services.AddScoped<ICommand, AddTask>();
            services.AddScoped<ICommand, GetTaskInfo>();
            services.AddScoped<ICommand, ChangeTaskStatusCommand>();

            services.AddSingleton<ICommand>(new StubCommand("Статистика по задачам (в разработке)"));
            services.AddSingleton<ICommand>(new TextCommand("Road map", @"
В скором времени, в боте появится новый функционал:
1. Напоминание о текущих задачах (с целью актуализации состояния)
2. Статистика по задачам
3. ...
"));
            services.AddScoped(provider => provider.GetServices<ICommand>().ToArray());


            void AddGetTaskTaskListCommand(TaskStatus status) => services.AddScoped<ICommand>(provider
                => new GetTaskTaskList(provider.GetService<ITaskHandler>(), status));
        }
    }
}