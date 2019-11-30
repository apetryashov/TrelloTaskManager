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
        public bool IsPublicCommand => true;

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
            return commandInfo.SessionMeta == null
                ? GetMenu("мы начинаем")
                : StartCommand(commandInfo.Author, commandInfo.Command, commandInfo.SessionMeta);
        }

        private ICommandResponse StartCommand(Author author, string commandText, ISessionMeta meta)
        {
            var response = GetResponse(author, commandText, meta);

            return response ?? new CommandResponse(new TextResponse("Кажется, что-то пошло не так :(", SessionStatus.Exception));
        }

        private ICommandResponse GetResponse(Author author, string commandText, ISessionMeta meta)
        {
            return (CommandStatus) meta.ContinueFrom switch
            {
                CommandStatus.Menu => ToMenuAction(author, commandText),
                CommandStatus.SetDescription => SetDescriptionAction(author, commandText),
                CommandStatus.SetName => SetNameAction(author, commandText),
                _ => throw new Exception() //add message
            };
        }

        private ICommandResponse ToMenuAction(Author author, string commandText)
        {
            return commandText switch
            {
                "Добавить название" => new CommandResponse(TextResponse.ExpectedCommand("Введите название"),
                    (int) CommandStatus.SetName),
                "Добавить описание" => new CommandResponse(TextResponse.ExpectedCommand("Введите описание"),
                    (int) CommandStatus.SetDescription),
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
            return new CommandResponse(new TextResponse("Название добавлено", SessionStatus.Expect), (int) CommandStatus.Menu);
        }

        private ICommandResponse SetDescriptionAction(Author author, string taskDescription)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            task.Description = taskDescription;
            taskInitializationStorage.Update(task);
            return new CommandResponse(TextResponse.ExpectedCommand("Описание добавлено"), (int) CommandStatus.Menu);
        }

        private ICommandResponse SaveAction(Author author)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            if (task.Name == default)
                return new CommandResponse(TextResponse.ExpectedCommand("Задаче необходимо добавить имя"),
                    (int) CommandStatus.Menu);
            taskInitializationStorage.Delete(task.Key);
            //trello saving logic

            return new CommandResponse(TextResponse.CloseCommand(@$"
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

            return new CommandResponse(TextResponse.AbortCommand(""));
        }

        private ICommandResponse GetMenu(string message)
        {
            return new CommandResponse(
                new ButtonResponse(message, menuCommands, SessionStatus.Expect),
                (int) CommandStatus.Menu
            );
        }

        private enum CommandStatus
        {
            Menu = 1,
            SetName = 2,
            SetDescription = 3
        }
    }
}