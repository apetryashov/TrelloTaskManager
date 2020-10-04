using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Common;
using TaskManager.Common.Tasks;
using TaskManager.Trello;

namespace TaskManager.Reminder
{
    public class UsersProvider : IUsersProvider
    {
        private readonly IUserItemsStorage<TrelloApiToken> trelloTokenStorage;
        private readonly ITaskHandler taskProvider;

        public UsersProvider(
            IUserItemsStorage<TrelloApiToken> trelloTokenStorage, 
            ITaskHandler taskProvider
            )
        {
            this.trelloTokenStorage = trelloTokenStorage;
            this.taskProvider = taskProvider;
        }

        public async IAsyncEnumerable<UserWithBoards> GetUsers()
        {
            foreach (var task in trelloTokenStorage.GetAllItems().Select(x => GetUserWithBoards(x.id, x.item)))
                yield return await task;
        }

        private async Task<UserWithBoards> GetUserWithBoards(long id, string userToken)
        {
            var boards = await taskProvider.GetAllBoardColumnsInfo(userToken);
            var requiredBoars = TrimStartAndFinishBoard(boards);
            return new UserWithBoards
            {
                TelegramId = id,
                UserToken = userToken,
                Boards = requiredBoars
            };
        }

        private static IEnumerable<BoardColumnInfo> TrimStartAndFinishBoard(
            IReadOnlyCollection<BoardColumnInfo> boardColumnInfos)
            => boardColumnInfos.Skip(1).Take(boardColumnInfos.Count - 2);
    }
}