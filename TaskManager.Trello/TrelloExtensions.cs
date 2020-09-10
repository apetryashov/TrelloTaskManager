using System.Threading.Tasks;
using Manatee.Trello;

namespace TaskManager.Trello
{
    public static class TrelloExtensions
    {
        public static async Task Move(this Card card, int position, List list = null)
        {
            if (list != null && list != card.List) card.List = list;

            card.Position = position;
            await card.Refresh();
        }

        public static Task<IMe> Me(this ITrelloFactory factory, string appKey, string userToken)
            => factory.Me(new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            });
    }
}