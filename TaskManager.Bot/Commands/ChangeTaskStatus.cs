using System.Text.RegularExpressions;
using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class ChangeTaskStatus : ICommandWithPrefixValidation
    {
        private readonly Regex expression =
            new Regex(@"/move_(?<targetColumnId>\w+)_(?<taskId>\w+)", RegexOptions.Compiled);

        private readonly ITaskHandler taskProvider;

        public ChangeTaskStatus(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

        public string CommandTrigger => "/move";

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var command = commandInfo.Command;
            var author = commandInfo.Author;
            var match = expression.Match(command);
            var targetColumnId = match.Groups["targetColumnId"].Value;
            var taskId = match.Groups["taskId"].Value;

            taskProvider.ChangeTaskColumn(author.UserToken, taskId, targetColumnId).Wait();

            return TextResponse.Create("Статус задачи изменен");
        }
    }
}