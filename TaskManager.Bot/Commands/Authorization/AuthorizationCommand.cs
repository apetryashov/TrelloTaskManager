using System.Threading.Tasks;
using TaskManager.Common;
using TelegramBot.Core.Commands;
using TelegramBot.Core.Domain;
using TelegramBot.Core.Model;

namespace TaskManager.Bot.Commands.Authorization
{
    //TODO: надо добавить reauthorize команду, которая удаляет из монги данные

    public class AuthorizationCommand : ICommandWithPrefixValidation
    {
        private readonly IAuthorizationProvider authorizationProvider;

        private readonly IResponse startCommandResponse = ChainResponse.Create()
            .AddResponse(TextResponse.Create(@"
Привет!
Давай для начала мы расскажем что это за бот и как он может помочь тебе."))
            .AddResponse(TextResponse.Create(@"
Данный бот создан с целью упростить и перенести в telegram самый популярный сценарии использования trello.
Почти все, кто создает доску в trello хотят решать задачу: новые задачи, задачи в прогрессе, сделанные задачи.

Если ты тоже заинтересован в решение такой задачи, то смело переходи к авторизации!
"));

        public AuthorizationCommand(
            IAuthorizationProvider authorizationProvider)
            => this.authorizationProvider = authorizationProvider;

        public Task<IResponse> StartCommand(ICommandInfo commandInfo)
        {
            var command = commandInfo.Command;
            var author = commandInfo.Author;
            return (command switch
            {
                "/start" => StartAuthorization(author, true),
                _ => StartAuthorization(author, false)
            }).RunAsTask();
        }

        public string CommandTrigger { get; } = "/authorize";

        private IResponse StartAuthorization(Author author, bool isStartCommand)
        {
            var chainResponse = ChainResponse.Create();

            if (isStartCommand)
                chainResponse.AddResponse(startCommandResponse);

            return chainResponse.AddResponse(GetHelpResponse(author));
        }

        private IResponse GetHelpResponse(Author author) =>
            LinkResponse.Create(
                "Чтобы пройти авторизацию, тебе нужно пройти лишь перейти по ссылке " +
                "и нажать кнопку <b>Разрешить</b>",
                "Авторизоваться",
                authorizationProvider.GetAuthorizationUrl(author));
    }
}