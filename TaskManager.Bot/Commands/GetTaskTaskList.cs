using System;
using System.Linq;
using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class GetTaskTaskList : ICommand
    {
        private readonly ITaskHandler taskProvider;
        private readonly TaskStatus taskStatus;

        public GetTaskTaskList(ITaskHandler taskProvider, TaskStatus taskStatus)
        {
            this.taskProvider = taskProvider;
            this.taskStatus = taskStatus;
        }

        public bool IsPublicCommand => true;

        public string CommandTrigger => taskStatus switch
        {
            TaskStatus.Active => "Все активные задачи",
            TaskStatus.Inactive => "Все неактивные задачи",
            TaskStatus.Resolved => "Все сделанные",
            _ => throw new ArgumentException($"unknown task status {taskStatus}")
        };

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var tasks = taskProvider.GetAllTasks(commandInfo.Author.UserToken, taskStatus).Result;

            var buttons = tasks.Select(x => (x.Name, $"/task_{x.Id}")).ToArray();
            return InlineButtonResponse.CreateWithVerticalButtons(CommandTrigger, buttons);
        }
    }
}