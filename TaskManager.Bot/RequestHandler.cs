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
        private readonly AuthorizationCommand authorizationCommand;
        private readonly IAuthorizationStorage authorizationStorage;
        private readonly ICommandWithPrefixValidation[] commandsWithPrefixValidation;
        private readonly ICommand defaultCommand;
        private readonly ITextButtonMenuProvider textButtonMenuProvider;

        public RequestHandler(
            IAuthorizationStorage authorizationStorage,
            AuthorizationCommand authorizationCommand,
            ICommandWithPrefixValidation[] commandsWithPrefixValidation,
            ICommand defaultCommand,
            ITextButtonMenuProvider textButtonMenuProvider)
        {
            this.authorizationStorage = authorizationStorage;
            this.commandsWithPrefixValidation = commandsWithPrefixValidation;
            this.authorizationCommand = authorizationCommand;
            this.defaultCommand = defaultCommand;
            this.textButtonMenuProvider = textButtonMenuProvider;
        }

        public IResponse GetResponse(IRequest request)
        {
            var author = request.Author;

            var commandText = request.Command;

            if (authorizationStorage.TryGetUserToken(author, out var token))
                author.UserToken = token;

            var response = Execute(author, commandText);

            if (response is TextResponse textResponse)
                response = textResponse.AsButton(GetMenu(author));

            // throw if not TextResponse?

            return response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            var commandInfo = new CommandInfo(author, commandText);

            var command = author.UserToken == null
                ? authorizationCommand
                : GetCommandByPrefix(commandText);

            return command.StartCommand(commandInfo);
        }

        private ICommand GetCommandByPrefix(string textCommand) =>
            commandsWithPrefixValidation
                .FirstOrDefault(c => c.IsValidCommand(textCommand))
            ?? defaultCommand;

        private string[] GetMenu(Author author) =>
            textButtonMenuProvider
                .GetButtons(author);
    }
}