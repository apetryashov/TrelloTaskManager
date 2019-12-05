using TaskManager.Bot.Telegram.Model;
using TaskManager.Common.Storage;

namespace TaskManager.Bot.Telegram.Commands
{
    public class GetTaskInfo : ICommand
    {
        private readonly ITaskProvider taskProvider;

        public GetTaskInfo(ITaskProvider taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public bool IsPublicCommand => false;
        public string CommandTrigger => "/task";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var taskId = int.Parse(commandInfo.Command.Substring(CommandTrigger.Length));
            var task = taskProvider.GetTaskById(commandInfo.Author.TelegramId, taskId);
            
            return new CommandResponse(TextResponse.CloseCommand(task.ToString()));
        }
    }
}