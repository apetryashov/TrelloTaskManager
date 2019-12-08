using TaskManager.Common.Tasks;
using TaskManaget.Bot.Model;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Commands
{
    public class GetTaskInfo : ICommand
    {
        private readonly ITaskHandler taskProvider;

        public GetTaskInfo(ITaskHandler taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public bool IsPublicCommand => false;
        public string CommandTrigger => "/task";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var taskId = commandInfo.Command.Substring(CommandTrigger.Length + 1);
            var task = taskProvider.GetTaskById(commandInfo.Author.UserToken, taskId).Result;

            var response = InlineButtonResponse.CreateWithHorizontalButtons(
                task.ToString(),
                GetButtons(task.Status, taskId),
                SessionStatus.Close
            );
            
            return new CommandResponse(response);
        }

        private (string text, string callback)[] GetButtons(TaskStatus status, string taskId)
        {
            return status switch
            {
                TaskStatus.Inactive => new (string text, string callback)[]
                {
                    ("-> Делаю", $"/changeTaskStatus_{TaskStatus.Active}_{taskId}"),
                    ("-> Сделал", $"/changeTaskStatus_{TaskStatus.Resolved}_{taskId}"),
                },
                TaskStatus.Active => new (string text, string callback)[]
                {
                    ("Сделаю <-", $"/changeTaskStatus_{TaskStatus.Inactive}_{taskId}"),
                    ("-> Сделал", $"/changeTaskStatus_{TaskStatus.Resolved}_{taskId}"),
                },
                TaskStatus.Resolved => new (string text, string callback)[]
                {
                    ("Сделаю <-", $"/changeTaskStatus_{TaskStatus.Inactive}_{taskId}"),
                    ("Делаю <-", $"/changeTaskStatus_{TaskStatus.Active}_{taskId}")
                }
            };
        }
    }
}