using System.Linq;
using TaskManager.Bot.Authorization;
using TaskManager.Bot.Model;
using TaskManager.Bot.Model.Domain;
using TaskManager.Bot.Model.Session;
using TaskManager.Common.Helpers;

namespace TaskManager.Bot
{
    public class RequestHandler : IRequestHandler
    {
        private const string AuthorizationCommand = "/authorize";
        private readonly IAuthorizationStorage authorizationStorage;
        private readonly ICommand[] commands;
        private readonly ISessionStorage sessionStorage;

        public RequestHandler(IAuthorizationStorage authorizationStorage, ICommand[] commands,
            ISessionStorage sessionStorage)
        {
            this.authorizationStorage = authorizationStorage;
            this.commands = commands;
            this.sessionStorage = sessionStorage;
        }

        public IResponse GetResponse(IRequest request)
        {
            var author = request.Author;

            var commandText = request.Command;
            var response = sessionStorage.TryGetUserSession(author, out var session)
                ? Execute(author, commandText, session)
                : Execute(author, commandText);

            if (response.SessionStatus != SessionStatus.Expect)
                if (response is TextResponse textResponse)
                    response = textResponse.AsButton(GetMenu());
            // throw if not TextResponse?

            return response;
        }

        private IResponse Execute(Author author, string commandText, ISession session)
        {
            if (!IsAuthorizationCommand(session))
            {
                authorizationStorage.TryGetUserToken(author, out var token);
                author.UserToken = token;
            }

            var command = commands[session.CommandId];
            var commandInfo = new CommandInfo(author, commandText, session.SessionMeta);
            var commandResponse = command.StartCommand(commandInfo);
            sessionStorage.HandleCommandSession(author, session.CommandId, commandResponse.Response.SessionStatus,
                commandResponse.SessionMeta);

            return commandResponse.Response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            if (!authorizationStorage.TryGetUserToken(author, out var token))
                commandText = AuthorizationCommand;
            else
                author.UserToken = token;

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
                if (textCommand.StartsWith(commands[index].CommandTrigger))
                    return (commands[index], index);

            return default;
        }

        private string[][] GetMenu() => commands
            .Where(x => x.IsPublicCommand)
            .Select(x => x.CommandTrigger)
            .ToArray()
            .AsDoubleArray(3);

        private bool IsAuthorizationCommand(ISession session)
            => commands[session.CommandId].CommandTrigger == AuthorizationCommand;
    }
}