using TaskManager.Bot.Model;
using TaskManager.Common;

namespace TaskManager.Bot.Commands.Authorization
{
    //TODO: надо добавить reauthorize команду, которая удаляет из монги данные

    public class AuthorizationCommand : ICommandWithPrefixValidation
    {
        private readonly IAuthorizationProvider authorizationProvider;

        private readonly IAuthorizationStorage authorizationStorage;

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
            IAuthorizationStorage authorizationStorage,
            IAuthorizationProvider authorizationProvider)
        {
            this.authorizationStorage = authorizationStorage;
            this.authorizationProvider = authorizationProvider;
        }

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var command = commandInfo.Command;
            return command switch
            {
                "/start" => StartAuthorization(true),
                "/authorize" => StartAuthorization(false),
                _ => ContinueAuthorization(commandInfo)
            };
        }

        public string CommandTrigger { get; } = "/authorize";

        private IResponse StartAuthorization(bool isStartCommand)
        {
            var chainResponse = ChainResponse.Create();

            if (isStartCommand)
                chainResponse.AddResponse(startCommandResponse);

            return chainResponse.AddResponse(GetHelpResponse());
        }

        private IResponse ContinueAuthorization(ICommandInfo commandInfo)
        {
            var token = commandInfo.Command;
            var errorResponse = ChainResponse.Create()
                .AddResponse(TextResponse.Create("Что-то пошло не так, попробуйте еще раз"))
                .AddResponse(GetHelpResponse());

            if (!authorizationProvider.IsValidAuthorizationToken(token).Result)
                return errorResponse;

            try
            {
                authorizationProvider.CheckOrInitializeWorkspace(token).GetAwaiter().GetResult();
                authorizationStorage.SetUserToken(commandInfo.Author, token);
                return TextResponse.Create(@"
Отлично! Авторизация успешно пройдена!

Теперь в твоем Trello аккаунте появилась новая таблица `TrelloTaskManager`.
В ней ты найдешь 3 листа, работа с которыми и происходит внутри этого бота.
Так же, ты можешь сам зайти на доску и добавить задачу в нужный тебе лист.
Данные автоматически будут синхронизированны.
");
            }
            catch
            {
                return errorResponse;
            }
        }

        private IResponse GetHelpResponse() =>
            TextResponse.Create(
                @$"
Чтобы пройти авторизацию, тебе нужно пройти лишь несколько шагов:
1. Прейти по ссылке {authorizationProvider.GetAuthorizationUrl()} и нажать кнопку <Разрешить>.
2. Затем нужно отправить в ответном сообщении полученный тобой токен.

Чуть позже этот процесс будет еще проще :(
");
    }
}