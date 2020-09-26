using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskManager.Common.Tasks;
using TelegramBot.Core.Commands;
using TelegramBot.Core.Model;

namespace TaskManager.Bot.Commands
{
    public class ChangeTaskStatus : ICommandWithPrefixValidation
    {
        private readonly Regex expression =
            new Regex(@"/move_(?<targetColumnId>\w+)_(?<taskId>\w+)", RegexOptions.Compiled);

        private readonly ITaskHandler taskProvider;

        public ChangeTaskStatus(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

        public string CommandTrigger => "/move";

        public async Task<IResponse> StartCommand(ICommandInfo commandInfo)
        {
            var command = commandInfo.Command;
            var author = commandInfo.Author;
            var match = expression.Match(command);
            var targetColumnId = match.Groups["targetColumnId"].Value;
            var taskId = match.Groups["taskId"].Value;

            await taskProvider.ChangeTaskColumn(author.TelegramId, taskId, targetColumnId);

            return TextResponse.Create("Статус задачи изменен");
        }
    }
}