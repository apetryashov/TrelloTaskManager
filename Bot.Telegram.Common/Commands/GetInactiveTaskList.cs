using System.Linq;
using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using TaskManager.Common;

namespace Bot.Telegram.Common.Commands
{
    public class GetInactiveTaskList : ICommand
    {
        private readonly ITaskProvider taskProvider;

        public GetInactiveTaskList(ITaskProvider taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public string CommandTrigger => "все активные задачи";

        public ICommandResponse StartCommand(Author author)
        {
            var tasksInfo = taskProvider.GetInactiveTasks(author.TelegramId)
                .Select(task => $"[{task.Name}] подробнее /task{task.Id}").ToArray();
            var response = new TextResponse($"Все активные задачи:\r\n{string.Join('\n', tasksInfo)}");

            return new CommandResponse(response);
        }

        public ICommandResponse StartCommand(Author author, string commandText, ISession commandSession)
        {
            throw new System.NotImplementedException();
        }
    }
}