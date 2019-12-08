using System;
using System.Text.RegularExpressions;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Bot.Telegram.Model.Session;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Telegram.Commands
{
    public class ChangeTaskStatusCommand : ICommand
    {
        private readonly ITaskHandler taskProvider;

        private readonly Regex expression =
            new Regex(@"/changeTaskStatus_(?<taskStatus>\w+)_(?<taskId>\w+)", RegexOptions.Compiled);
        
        public ChangeTaskStatusCommand(ITaskHandler taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public bool IsPublicCommand => false;
        public string CommandTrigger => "/changeTaskStatus";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var command = commandInfo.Command;
            var author = commandInfo.Author;
            var match = expression.Match(command);
            var taskStatus = match.Groups["taskStatus"].Value;
            Enum.TryParse<TaskStatus>(taskStatus, out var status);
            var taskId = match.Groups["taskId"].Value;

            var myTask = taskProvider.GetTaskById(author.UserToken, taskId).Result;
            taskProvider.ChangeTaskStatus(author.UserToken, myTask, status).Wait();

            return new CommandResponse(new TextResponse($"Статус задачи изменен на: {status}", SessionStatus.Close));
        }
    }
}