using System.Linq;
using TaskManager.Bot.Telegram.Commands;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Bot.Telegram.Model.Domain;
using TaskManager.Bot.Telegram.Model.Session;
using TaskManager.Common.Helpers;
using TaskManager.Common.Storage;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskManager.Bot.Telegram
{
    public class RequestHandler : IRequestHandler
    {
        private const string AuthorizationCommand = "/authorize";
        private readonly ICommand[] commands;
        private readonly ISessionStorage sessionStorage = new InMemorySessionStorage();
        private readonly AuthorizationStorage authorizationStorage;

            public RequestHandler(ITaskProvider taskProvider, string appKey)
            {
                authorizationStorage = new AuthorizationStorage();
                commands = new ICommand[]
            {
                new AuthorizationCommand(authorizationStorage, appKey),
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
            if (!IsAuthorizationCommand(session))
                author.UserToken = authorizationStorage.GetUserToken(author);

            var command = commands[session.CommandId];
            var commandInfo = new CommandInfo(author, commandText, session.SessionMeta);
            var commandResponse = command.StartCommand(commandInfo);
            sessionStorage.HandleCommandSession(author, session.CommandId, commandResponse.Response.SessionStatus,
                commandResponse.SessionMeta);

            return commandResponse.Response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            if (!authorizationStorage.IsAuthorizedUser(author))
                commandText = AuthorizationCommand;
            else
                author.UserToken = authorizationStorage.GetUserToken(author);

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

        private bool IsAuthorizationCommand(ISession session)
        {
            return commands[session.CommandId].CommandTrigger == AuthorizationCommand;
        }
    }
}