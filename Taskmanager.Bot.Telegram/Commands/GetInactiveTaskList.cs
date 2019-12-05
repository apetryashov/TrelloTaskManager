using System.Linq;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Telegram.Commands
{
    public class GetInactiveTaskList : ICommand
    {
        public bool IsPublicCommand => true;
        private readonly ITaskHandler taskProvider;

        public GetInactiveTaskList(ITaskHandler taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public string CommandTrigger => "все активные задачи";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var tasksInfo = taskProvider.GetAllTasks(commandInfo.Author.UserToken, TaskStatus.Inactive).Result
                .Select(task => $"[{task.Name}] подробнее /task_{task.Id}").ToArray();
            var response = TextResponse.CloseCommand($"Все активные задачи:\r\n{string.Join('\n', tasksInfo)}");

            return new CommandResponse(response);
        }
    }
}