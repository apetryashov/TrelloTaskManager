using System;
using TaskManager.Common.Storage;
using TaskManager.Common.Tasks;
using TaskManaget.Bot.Model;
using TaskManaget.Bot.Model.Domain;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Commands
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

        private readonly ITaskHandler taskProvider;
        private readonly InMemoryStorage<MyTask, long> taskInitializationStorage;

        public AddTask(ITaskHandler taskProvider)
        {
            this.taskProvider = taskProvider;
            taskInitializationStorage = new InMemoryStorage<MyTask, long>();
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

            return response ?? new CommandResponse(TextResponse.CloseCommand("Кажется, что-то пошло не так :("));
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
            return GetMenu("Название добавлено");
        }

        private ICommandResponse SetDescriptionAction(Author author, string taskDescription)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            task.Description = taskDescription;
            taskInitializationStorage.Update(task);
            return GetMenu("Описание добавлено");
        }

        private ICommandResponse SaveAction(Author author)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            if (task.Name == default)
                return new CommandResponse(TextResponse.ExpectedCommand("Задаче необходимо добавить имя"),
                    (int) CommandStatus.Menu);
            taskInitializationStorage.Delete(task.Key);
            //trelloAuthorizationProvider saving logic
            taskProvider.AddNewTask(author.UserToken, task).Wait();
            return new CommandResponse(TextResponse.CloseCommand(@$"
Задача успешно добавлена!

{task}
"));
        }
        
        private ICommandResponse AbortAction(Author author)
        {
            var task = taskInitializationStorage.Get(author.TelegramId);
            taskInitializationStorage.Delete(task.Key);

            return new CommandResponse(TextResponse.AbortCommand("Отменено"));
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