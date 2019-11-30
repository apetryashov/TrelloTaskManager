using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Storage;

namespace Bot.Telegram.Common.Commands
{
    public class GetTaskInfo : ICommand
    {
        private readonly ITaskProvider taskProvider;

        public GetTaskInfo(ITaskProvider taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public string CommandTrigger => "/task";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var taskId = int.Parse(commandInfo.Command.Substring(CommandTrigger.Length));
            var task = taskProvider.GetTaskById(commandInfo.Author.TelegramId, taskId);
            
            return new CommandResponse(new TextResponse(@$"
[{task.Name}]
{task.Description}
"));
        }
    }
}