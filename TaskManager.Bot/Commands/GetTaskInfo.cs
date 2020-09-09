using System.Collections.Generic;
using System.Linq;
using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class GetTaskInfo : ICommandWithPrefixValidation
    {
        private readonly ITaskHandler taskProvider;

        public GetTaskInfo(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

        public string CommandTrigger => "/task";

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var taskId = commandInfo.Command.Substring(CommandTrigger.Length + 1);
            var token = commandInfo.Author.UserToken;
            var task = taskProvider.GetTaskById(commandInfo.Author.UserToken, taskId).Result;
            var allBoards = taskProvider.GetAllBoardColumnsInfo(token).Result;

            return InlineButtonResponse.CreateWithHorizontalButtons(
                task.ToString(),
                GetButtons(task, allBoards).ToArray()
            );
        }

        private static IEnumerable<(string text, string callback)> GetButtons(
            MyTask task,
            IEnumerable<BoardColumnInfo> allColumns)
        {
            var taskColumnIsFound = false;
            foreach (var column in allColumns)
            {
                var targetColumnName = column.Name;
                var targetColumnId = column.Id;
                var taskId = task.Id;
                if (targetColumnName == task.Status)
                {
                    taskColumnIsFound = true;
                    continue;
                }

                var changeTaskCommand = $"/move_{targetColumnId}_{taskId}";

                yield return taskColumnIsFound
                    ? ($"-> {targetColumnName}", changeTaskCommand)
                    : ($"{targetColumnName} <-", changeTaskCommand);
            }
        }
    }
}