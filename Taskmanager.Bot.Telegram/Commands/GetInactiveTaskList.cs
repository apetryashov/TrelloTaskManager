using System.Linq;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Common.Storage;

namespace TaskManager.Bot.Telegram.Commands
{
    public class GetInactiveTaskList : ICommand
    {
        public bool IsPublicCommand => true;
        private readonly ITaskProvider taskProvider;

        public GetInactiveTaskList(ITaskProvider taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public string CommandTrigger => "все активные задачи";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var tasksInfo = taskProvider.GetInactiveTasks(commandInfo.Author.TelegramId)
                .Select(task => $"[{task.Name}] подробнее /task{task.Id}").ToArray();
            var response = TextResponse.CloseCommand($"Все активные задачи:\r\n{string.Join('\n', tasksInfo)}");

            return new CommandResponse(response);
        }
    }
}