using System;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using TaskManager.Common;
using TelegramBot.Core.Domain;

namespace TaskManager.Trello
{
    public class TrelloAuthorizationProvider : IAuthorizationProvider
    {
        private const string SystemTableName = "TrelloTaskManager";
        private readonly string appKey;
        private readonly ITrelloFactory factory;
        private readonly string returnUrl;

        public TrelloAuthorizationProvider(
            AppKey appKey,
            ReturnUrl returnUrl,
            ITrelloFactory trelloFactory)
        {
            this.appKey = appKey.Key;
            this.returnUrl = returnUrl.Url;
            factory = trelloFactory;
        }

        public async Task<bool> IsValidAuthorizationToken(string userToken)
        {
            try
            {
                await factory.Me(appKey, userToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CheckOrInitializeWorkspace(string userToken)
        {
            var me = await factory.Me(appKey, userToken);
            var boardCollection = me.Boards;
            await boardCollection.Refresh();
            var board = boardCollection.FirstOrDefault(x => x.Name == SystemTableName);

            if (board == default)
                await me.Boards.Add(SystemTableName);
        }

        public Uri GetAuthorizationUrl(Author author)
            => new Uri("https://trello.com/1/authorize" +
                       "?expiration=never" +
                       "&scope=read,write,account" +
                       "&response_type=token" +
                       "&name=TrelloTaskManager" +
                       $"&key={appKey}" +
                       $"&return_url={returnUrl}/auth?telegram_id={author.TelegramId}");
    }
}