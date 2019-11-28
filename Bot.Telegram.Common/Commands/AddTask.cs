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
        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            if (commandInfo.Session == null)
            {
                return new CommandResponse(new TextResponse("Введите название задачи"),
                    CommandSession.ExpectCommandSession((int) SessionStatus.EditCommandName));
            }

            return StartCommand(commandInfo.Author, commandInfo.Command, commandInfo.Session);
        }

        private ICommandResponse StartCommand(Author author, string commandText, ISession commandSession)
        {
            switch ((SessionStatus) commandSession.ContinueIndex)
            {
                case SessionStatus.EditCommandName:
                    var task = new Task
                    {
                        Name = commandText
                    };
                    taskProvider.AddNewTask(author.TelegramId, task);
                    return new CommandResponse(new TextResponse("задача успешно добавлена"));
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