using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class GetTaskInfo : ICommand
    {
        private readonly ITaskHandler taskProvider;

        public GetTaskInfo(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

        public bool IsPublicCommand => false;
        public string CommandTrigger => "/task";

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var taskId = commandInfo.Command.Substring(CommandTrigger.Length + 1);
            var task = taskProvider.GetTaskById(commandInfo.Author.UserToken, taskId).Result;

            return InlineButtonResponse.CreateWithHorizontalButtons(
                task.ToString(),
                GetButtons(task.Status, taskId)
            );
        }

        private (string text, string callback)[] GetButtons(TaskStatus status, string taskId) => status switch
        {
            TaskStatus.Inactive => new (string text, string callback)[]
            {
                ("-> Делаю", $"/changeTaskStatus_{TaskStatus.Active}_{taskId}"),
                ("-> Сделал", $"/changeTaskStatus_{TaskStatus.Resolved}_{taskId}")
            },
            TaskStatus.Active => new (string text, string callback)[]
            {
                ("Сделаю <-", $"/changeTaskStatus_{TaskStatus.Inactive}_{taskId}"),
                ("-> Сделал", $"/changeTaskStatus_{TaskStatus.Resolved}_{taskId}")
            },
            TaskStatus.Resolved => new (string text, string callback)[]
            {
                ("Сделаю <-", $"/changeTaskStatus_{TaskStatus.Inactive}_{taskId}"),
                ("Делаю <-", $"/changeTaskStatus_{TaskStatus.Active}_{taskId}")
            }
        };
    }
}