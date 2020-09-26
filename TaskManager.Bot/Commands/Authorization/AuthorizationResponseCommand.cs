using System.Threading.Tasks;
using TaskManager.Common;
using TaskManager.Trello;
using TelegramBot.Core.Model;

namespace TaskManager.Bot.Commands.Authorization
{
    public class AuthorizationResponseCommand
    {
        private readonly IAuthorizationProvider authorizationProvider;
        private readonly IUserItemsStorage<TrelloApiToken> userTokenStorage;

        public AuthorizationResponseCommand(
            IAuthorizationProvider authorizationProvider,
            IUserItemsStorage<TrelloApiToken> userTokenStorage)
        {
            this.authorizationProvider = authorizationProvider;
            this.userTokenStorage = userTokenStorage;
        }

        public async Task<IResponse> StartCommand(long telegramId, string token)
        {
            var errorResponse = ChainResponse.Create()
                .AddResponse(TextResponse.Create("Что-то пошло не так, попробуйте еще раз"));

            var isValidToken = await authorizationProvider.IsValidAuthorizationToken(token);

            if (!isValidToken)
                return errorResponse;

            try
            {
                await authorizationProvider.CheckOrInitializeWorkspace(token);
                userTokenStorage.Set(telegramId, new TrelloApiToken
                {
                    Token = token
                });
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
    }
}