using System;
using System.Text.RegularExpressions;
using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class ChangeTaskStatusCommand : ICommand
    {
        private readonly Regex expression =
            new Regex(@"/changeTaskStatus_(?<taskStatus>\w+)_(?<taskId>\w+)", RegexOptions.Compiled);

        private readonly ITaskHandler taskProvider;

        public ChangeTaskStatusCommand(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

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

            return new CommandResponse(
                TextResponse.CloseCommand($"Статус задачи изменен на: {status.AsPrintableString()}"));
        }
    }
}