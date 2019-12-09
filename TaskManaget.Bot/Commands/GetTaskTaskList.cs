using System;
using System.Linq;
using TaskManager.Common.Tasks;
using TaskManaget.Bot.Model;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Commands
{
    public class GetTaskTaskList : ICommand
    {
        public bool IsPublicCommand => true;
        private readonly ITaskHandler taskProvider;
        private readonly TaskStatus taskStatus;

        public GetTaskTaskList(ITaskHandler taskProvider, TaskStatus taskStatus)
        {
            this.taskProvider = taskProvider;
            this.taskStatus = taskStatus;
        }

        public string CommandTrigger => taskStatus switch
        {
            TaskStatus.Active => "Все активные задачи",
            TaskStatus.Inactive => "Все неактивные задачи",
            TaskStatus.Resolved => "Все сделанные",
            _ => throw new ArgumentException($"unknown task status {taskStatus}")
        };

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var tasks = taskProvider.GetAllTasks(commandInfo.Author.UserToken, taskStatus).Result;

            var buttons = tasks.Select(x => (x.Name, $"/task_{x.Id}")).ToArray();
            var response  = InlineButtonResponse.CreateWithVerticalButtons(CommandTrigger, buttons, SessionStatus.Close);
            return new CommandResponse(response);
        }
    }
}