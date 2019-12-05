using TaskManager.Bot.Telegram.Model;
using TaskManager.Common.Storage;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Telegram.Commands
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
            var task = taskProvider.GetTaskById(commandInfo.Author.UserToken, taskId);
            
            return new CommandResponse(TextResponse.CloseCommand(task.ToString()));
        }
    }
}