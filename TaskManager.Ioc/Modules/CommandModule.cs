using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Bot.Commands;
using TaskManager.Bot.Commands.Authorization;
using TelegramBot.Core.Commands;

namespace TaskManager.Ioc.Modules
{
    public class CommandModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<AuthorizationResponseCommand>();
            services.AddScoped<ICommandWithPrefixValidation, GetTaskInfo>();
            services.AddScoped<ICommandWithPrefixValidation, AuthorizationCommand>();
            services.AddScoped<ICommandWithPrefixValidation, HelpCommand>();
            services.AddScoped<ICommandWithPrefixValidation, ChangeTaskStatus>();
            services.AddScoped(provider => provider.GetServices<ICommandWithPrefixValidation>().ToArray());
            services.AddScoped<AddTask>();
            services.AddScoped<ICommand, GetTaskListOrAddTaskCommand>();
            services.AddScoped<AuthorizationCommand>();
        }
    }
}