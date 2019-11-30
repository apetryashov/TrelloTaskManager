using System;
using System.Linq;
using Bot.Telegram.Common.Commands;
using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using Bot.Telegram.Common.Storage;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Telegram.Common
{
    public class RequestHandler : IRequestHandler
    {
        private readonly ICommand[] commands;
        private readonly ISessionStorage sessionStorage = new InMemorySessionStorage();

        public RequestHandler(ITaskProvider taskProvider)
        {
            commands = new ICommand[]
            {
                new GetInactiveTaskList(taskProvider),
                new AddTask(taskProvider),
                new GetTaskInfo(taskProvider),
                new StubCommand("Статистика по задачам (в разработке)"),
                new StubCommand("help (в разработке)"),
            };
        }

        public IResponse GetResponse(IRequest request)
        {
            var author = request.Author;
            var commandText = request.Command;

            var response = sessionStorage.TryGetUserSession(author, out var session)
                ? Execute(author, commandText, session)
                : Execute(author, commandText);

            if (response.SessionStatus != SessionStatus.Expect)
            {
                if (response is TextResponse textResponse)
                    response = textResponse.AsButton(GetMenu());
                // throw if not TextResponse?
            }

            return response;
        }

        private IResponse Execute(Author author, string commandText, ISession session)
        {
            var command = commands[session.CommandId];
            var commandInfo = new CommandInfo(author, commandText, session.SessionMeta);
            var commandResponse = command.StartCommand(commandInfo);
            sessionStorage.HandleCommandSession(author, session.CommandId, commandResponse.Response.SessionStatus,
                commandResponse.SessionMeta);

            return commandResponse.Response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            var (command, commandIndex) = GetCommandByPrefix(commandText);

            if (command == default)
                return new ButtonResponse("сделай правильный вабор", GetMenu(), SessionStatus.Close);

            var commandInfo = new CommandInfo(author, commandText);
            var commandResponse = command.StartCommand(commandInfo);
            sessionStorage.HandleCommandSession(author, commandIndex, commandResponse.Response.SessionStatus,
                commandResponse.SessionMeta);

            return commandResponse.Response;
        }

        private (ICommand command, int commandIndex) GetCommandByPrefix(string textCommand)
        {
            for (var index = 0; index < commands.Length; index++)
            {
                if (textCommand.StartsWith(commands[index].CommandTrigger))
                    return (commands[index], index);
            }

            return default;
        }

        private ReplyKeyboardMarkup GetMenu()
        {
            return commands
                .Where(x => x.IsPublicCommand)
                .Select(x => x.CommandTrigger)
                .ToArray()
                .AsDoubleArray(2);
            ;
        }
    }
    
    public static class ArrayExtension
    {
        public static T[][] AsDoubleArray<T>(this T[] array, int columnsCount)
        {
            var rowsCount = (int)Math.Ceiling(((double)array.Length / columnsCount));

            var result = new T[rowsCount][];

            for (var i = 0; i < rowsCount; i++)
            {
                var cc = Math.Min(array.Length - (i * columnsCount), columnsCount);
                result[i] = new T[columnsCount];
                for (var j = 0; j < cc; j++)
                {
                    result[i][j] = array[i * columnsCount + j];
                }
            }

            return result;
        }
    }
}