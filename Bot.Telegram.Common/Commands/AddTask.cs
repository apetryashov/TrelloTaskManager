using System;
using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using Bot.Telegram.Common.Storage;

namespace Bot.Telegram.Common.Commands
{
    public class AddTask : ICommand
    {
        private readonly string[][] menuCommands =
        {
            new[]
            {
                "Добавить название",
                "Добавить описание"
            },
            new[]
            {
                "Сохранить",
                "Отмена"
            },
        };

        private readonly ITaskProvider taskProvider;
        private readonly InMemoryStorage<Task, long> taskInitializationStorage;

        public AddTask(ITaskProvider taskProvider)
        {
            this.taskProvider = taskProvider;
            taskInitializationStorage = new InMemoryStorage<Task, long>();
        }

        public string CommandTrigger => "добавить задачу";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            return commandInfo.Session == null
                ? GetMenu("мы начинаем")
                : StartCommand(commandInfo.Author, commandInfo.Command, commandInfo.Session);
        }

        private ICommandResponse StartCommand(Author author, string commandText, ISession commandSession)
        {
            var response = GetResponse(author, commandText, commandSession);

            return response ?? new CommandResponse(new TextResponse("Кажется, что-то пошло не так :("),
                       CommandSession.ErrorCommandSession());
        }

        private ICommandResponse GetResponse(Author author, string commandText, ISession commandSession)
        {
            return (SessionStatus) commandSession.ContinueIndex switch
            {
                SessionStatus.Menu => ToMenuAction(author, commandText),
                SessionStatus.SetDescription => SetDescriptionAction(author, commandText),
                SessionStatus.SetName => SetNameAction(author, commandText),
                _ => throw new Exception() //add message
            };
        }

        private ICommandResponse ToMenuAction(Author author, string commandText)
        {
            return commandText switch
            {
                "Добавить название" => new CommandResponse(new TextResponse("Введите название"),
                    (int) SessionStatus.SetName),
                "Добавить описание" => new CommandResponse(new TextResponse("Введите описание"),
                    (int) SessionStatus.SetDescription),
                "Сохранить" => SaveAction(author),
                "Отмена" => AbortAction(author),
                _ => GetMenu("Попробуй еще")
            };
        }

        private ICommandResponse SetNameAction(Author author, string taskName)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            task.Name = taskName;
            taskInitializationStorage.Update(task);
            return new CommandResponse(new TextResponse("Название добавлено"), (int) SessionStatus.Menu);
        }

        private ICommandResponse SetDescriptionAction(Author author, string taskDescription)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            task.Description = taskDescription;
            taskInitializationStorage.Update(task);
            return new CommandResponse(new TextResponse("Описание добавлено"), (int) SessionStatus.Menu);
        }

        private ICommandResponse SaveAction(Author author)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            if (task.Name == default)
                return new CommandResponse(new TextResponse("Задаче необходимо добавить имя"),
                    (int) SessionStatus.Menu);
            taskInitializationStorage.Delete(task.Key);
            //trello saving logic

            return new CommandResponse(new TextResponse(@$"
Задача успешно добавлена!
[Название]: {task.Name}
[Описание]: {task.Description}
Найти задачу вы всегда можете вызвав комманду /task{task.Id}
Так же вы можете найти ее на trello доске по ссылке (тут должна быть ссылка)"));
        }
        
        private ICommandResponse AbortAction(Author author)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            taskInitializationStorage.Delete(task.Key);

            return new CommandResponse(null, CommandSession.AbortCommandSession());
        }

        private ICommandResponse GetMenu(string message)
        {
            return new CommandResponse(
                new ButtonResponse(message, menuCommands),
                (int) SessionStatus.Menu
            );
        }

        private enum SessionStatus
        {
            Menu = 1,
            SetName = 2,
            SetDescription = 3
        }
    }
}