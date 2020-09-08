using System.Linq;
using TaskManager.Bot.Authorization;
using TaskManager.Bot.Commands;
using TaskManager.Bot.Model;
using TaskManager.Bot.Model.Domain;
using TaskManager.Common.Helpers;

namespace TaskManager.Bot
{
    public class RequestHandler : IRequestHandler
    {
        private const string AuthorizationCommand = "/authorize";
        private readonly AuthorizationCommand authorizationCommand;
        private readonly IAuthorizationStorage authorizationStorage;
        private readonly ICommand[] commands;
        private readonly IDefaultCommand defaultCommand;

        public RequestHandler(
            IAuthorizationStorage authorizationStorage,
            AuthorizationCommand authorizationCommand,
            ICommand[] commands,
            IDefaultCommand defaultCommand)
        {
            this.authorizationStorage = authorizationStorage;
            this.commands = commands;
            this.authorizationCommand = authorizationCommand;
            this.defaultCommand = defaultCommand;
        }

        public IResponse GetResponse(IRequest request)
        {
            var author = request.Author;

            var commandText = request.Command;
            var response = Execute(author, commandText);

            if (response is TextResponse textResponse)
                response = textResponse.AsButton(GetMenu());

            // throw if not TextResponse?

            return response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            if (authorizationStorage.TryGetUserToken(author, out var token))
                author.UserToken = token;

            var commandInfo = new CommandInfo(author, commandText);

            var command = token == null
                ? authorizationCommand
                : GetCommandByPrefix(commandText);

            return command.StartCommand(commandInfo);
        }

        private IDefaultCommand GetCommandByPrefix(string textCommand)
            => commands.FirstOrDefault(command => textCommand.StartsWith(command.CommandTrigger))
               ?? defaultCommand;

        private string[][] GetMenu() => commands
            .Where(x => x.IsPublicCommand)
            .Select(x => x.CommandTrigger)
            .ToArray()
            .AsDoubleArray(3);
    }
}