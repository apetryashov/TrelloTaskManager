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
            var taskStatuses = Enum
                .GetValues(typeof(TaskStatus))
                .Cast<TaskStatus>()
                .ToList();

            taskStatuses
                .ForEach(AddGetTaskTaskListCommand);

            services.AddScoped<ICommand, GetTaskInfo>();
            services.AddScoped<ICommand, AuthorizationCommand>();
            services.AddScoped<ICommand, HelpCommand>();
            services.AddScoped<ICommand, ChangeTaskStatusCommand>();
            services.AddScoped(provider => provider.GetServices<ICommand>().ToArray());
            services.AddScoped<IDefaultCommand, AddTask>();
            services.AddScoped<AuthorizationCommand>();

            void AddGetTaskTaskListCommand(TaskStatus status) => services.AddScoped<ICommand>(provider
                => new GetTaskTaskList(provider.GetService<ITaskHandler>(), status));
        }
    }
}