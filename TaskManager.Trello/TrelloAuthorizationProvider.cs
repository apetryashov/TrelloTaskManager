using System;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using TaskManager.Common;

namespace TaskManager.Trello
{
    public class TrelloAuthorizationProvider : IAuthorizationProvider
    {
        private const string SystemTableName = "TrelloTaskManager";
        private readonly string appKey;
        private readonly ITrelloFactory factory;
        //TODO: дать возможность создавать любую доску.
        //Для этого можно ввести команду /refresh_board, которая переподтягивает доску

        public TrelloAuthorizationProvider(AppKey appKey, ITrelloFactory trelloFactory)
        {
            this.appKey = appKey.Key;
            factory = trelloFactory;
        }

        public async Task<bool> IsValidAuthorizationToken(string userToken)
        {
            try
            {
                await GetMe(userToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CheckOrInitializeWorkspace(string userToken)
        {
            var me = await GetMe(userToken);
            var boardCollection = me.Boards;
            await boardCollection.Refresh();
            var board = boardCollection.FirstOrDefault(x => x.Name == SystemTableName);

            if (board == default)
                await me.Boards.Add(SystemTableName);
        }

        public string GetAuthorizationUrl()
            => $"https://trello.com/1/authorize?expiration=never&scope=read,write,account&response_type=token&name=TrelloTaskManager&key={appKey}";

        private async Task<IMe> GetMe(string userToken)
        {
            TrelloAuthorization.Default.AppKey =
                appKey; // it will not work with multithreading. https://github.com/gregsdennis/Manatee.TrelloAuthorizationProvider/issues/313
            TrelloAuthorization.Default.UserToken = userToken;

            return await factory.Me();
        }
    }
}