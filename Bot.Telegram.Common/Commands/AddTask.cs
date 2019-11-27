using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using TaskManager.Common;

namespace Bot.Telegram.Common.Commands
{
    public class AddTask : ICommand
    {
        private readonly ITaskProvider taskProvider;

        public AddTask(ITaskProvider taskProvider)
        {
            this.taskProvider = taskProvider;
        }

        public string CommandTrigger => "добавить задачу";

        public ICommandResponse StartCommand(Author author)
        {
            return new CommandResponse(new TextResponse("Введите название задачи"),
                CommandSession.ExpectCommandSession((int) SessionStatus.EditCommandName));
        }

        public ICommandResponse StartCommand(Author author, string commandText, ISession commandSession)
        {
            if ((SessionStatus) commandSession.ContinueIndex == SessionStatus.EditCommandName)
            {
                taskProvider.AddNewTask(author.TelegramId, new Task
                {
                    Name = commandText
                });

                return new CommandResponse(new TextResponse("Задача успешно добавлена"),
                    CommandSession.SimpleCommandSession());
            }

            return new CommandResponse(new TextResponse("Кажется, что-то пошло не так :("),
                CommandSession.ErrorCommandSession());
        }

        private enum SessionStatus
        {
            EditCommandName = 1
        }
    }
}