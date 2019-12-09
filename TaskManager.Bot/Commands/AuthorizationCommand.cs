using TaskManager.Bot.Authorization;
using TaskManager.Bot.Model;
using TaskManager.Bot.Model.Session;
using TaskManager.Common;

namespace TaskManager.Bot.Commands
{
    public class AuthorizationCommand : ICommand
    {
        private readonly IAuthorizationProvider authorizationProvider;

        private readonly IAuthorizationStorage authorizationStorage;

        public AuthorizationCommand(IAuthorizationStorage authorizationStorage,
            IAuthorizationProvider authorizationProvider)
        {
            this.authorizationStorage = authorizationStorage;
            this.authorizationProvider = authorizationProvider;
        }

        public bool IsPublicCommand => false;
        public string CommandTrigger => "/authorize";

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            return commandInfo.SessionMeta == null ? StartAuthorization() : ContinueAuthorization(commandInfo);
        }

        private ICommandResponse StartAuthorization()
        {
            var message = @"
Данный бот создан с целью упростить и перенести в telegram самый популярный сценарии использования trello.
Почти все, кто создает доску в trello хотят решать задачу: новые задачи, задачи в прогрессе, сделанные задачи.

Если ты тоже заинтересован в решение такой задачи, то смело переходи к авторизации!
";
            var response = ChainResponse.Create(SessionStatus.Expect)
                .AddResponse(TextResponse.ExpectedCommand(@"
Привет!
Давай для начала мы расскажем что это за бот и как он может помочь тебе."))
                .AddResponse(TextResponse.ExpectedCommand(message))
                .AddResponse(GetHelpResponse());

            return new CommandResponse(response, (int) CommandStatus.AuthorizationRequest);
        }

        private ICommandResponse ContinueAuthorization(ICommandInfo commandInfo)
        {
            var token = commandInfo.Command;


            if (authorizationProvider.IsValidAuthorizationToken(token).Result)
            {
                authorizationProvider.CheckOrInitializeWorkspace(token).GetAwaiter().GetResult();
                authorizationStorage.SetUserToken(commandInfo.Author, token);
                return new CommandResponse(TextResponse.CloseCommand(@"
Отлично! Авторизация успешно пройдена!

Теперь в твоем Trello аккаунте появилась новая таблица `TrelloTaskManager`.
В ней ты найдешь 3 листа, работа с которыми и происходит внутри этого бота.
Так же, ты можешь сам зайти на доску и добавить задачу в нужный тебе лист.
Данные автоматически будут синхронизированны.
"));
            }

            var response = ChainResponse.Create(SessionStatus.Expect)
                .AddResponse(TextResponse.ExpectedCommand("Что-то пошло не так, попробуйте еще раз"))
                .AddResponse(GetHelpResponse());

            return new CommandResponse(response, (int) CommandStatus.AuthorizationError);
        }

        private IResponse GetHelpResponse()
        {
            return TextResponse.ExpectedCommand(
                @$"
Чтобы пройти авторизацию, тебе нужно пройти лишь несколько шагов:
1. Прейти по ссылке {authorizationProvider.GetAuthorizationUrl()} и нажать кнопку <Разрешить>.
2. Затем нужно отправить в ответном сообщении полученный тобой токен.

Чуть позже этот процесс будет еще проще :(
");
        }

        private enum CommandStatus
        {
            AuthorizationRequest,
            AuthorizationError
        }
    }
}