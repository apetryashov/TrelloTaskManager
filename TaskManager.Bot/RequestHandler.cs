using System.Linq;
using TaskManager.Bot.Commands.Authorization;
using TaskManager.Common;
using TaskManager.Trello;
using TelegramBot.Core.Commands;
using TelegramBot.Core.Domain;
using TelegramBot.Core.Model;

namespace TaskManager.Bot
{
    public class RequestHandler : IRequestHandler
    {
        private readonly AuthorizationCommand authorizationCommand;
        private readonly IUserItemsStorage<TrelloApiToken> userTokenStorage;
        private readonly ICommandWithPrefixValidation[] commandsWithPrefixValidation;
        private readonly ICommand defaultCommand;
        private readonly ITextButtonMenuProvider textButtonMenuProvider;

        public RequestHandler(
            IUserItemsStorage<TrelloApiToken> userTokenStorage,
            AuthorizationCommand authorizationCommand,
            ICommandWithPrefixValidation[] commandsWithPrefixValidation,
            ICommand defaultCommand,
            ITextButtonMenuProvider textButtonMenuProvider)
        {
            this.userTokenStorage = userTokenStorage;
            this.commandsWithPrefixValidation = commandsWithPrefixValidation;
            this.authorizationCommand = authorizationCommand;
            this.defaultCommand = defaultCommand;
            this.textButtonMenuProvider = textButtonMenuProvider;
        }

        public IResponse GetResponse(IRequest request)
        {
            var author = request.Author;

            var commandText = request.Command;
            var response = Execute(author, commandText);

            if (response is TextResponse textResponse)
                response = textResponse.AsButton(GetMenu(author));

            // throw if not TextResponse?

            return response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            var commandInfo = new CommandInfo(author, commandText);

            var command = userTokenStorage.Has(author.TelegramId)
                ? GetCommandByPrefix(commandText)
                : authorizationCommand;

            return command.StartCommand(commandInfo);
        }

        private ICommand GetCommandByPrefix(string textCommand) =>
            commandsWithPrefixValidation
                .FirstOrDefault(c => c.IsValidCommand(textCommand))
            ?? defaultCommand;

        private string[] GetMenu(Author author) =>
            textButtonMenuProvider
                .GetButtons(author.TelegramId);
    }
}