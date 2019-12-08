using System;
using System.Linq;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Bot.Telegram.Model.Session;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Telegram.Commands
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

        // public string CommandTrigger => "все активные задачи";
        public string CommandTrigger => taskStatus switch
        {
            TaskStatus.Active => "Все активные задачи",
            TaskStatus.Inactive => "Все не активные задачи",
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