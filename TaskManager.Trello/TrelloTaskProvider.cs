using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using TaskManager.Common.Storage;

namespace TaskManager.Trello
{
    public class TrelloTaskProvider // refactor this
    {
        private readonly string appKey;
        private readonly ITrelloFactory factory;
        private readonly Dictionary<string, BoardInfo> userTokenToBoardInfo = new Dictionary<string, BoardInfo>();

        public TrelloTaskProvider(string appKey)
        {
            this.appKey = appKey;
            factory = new TrelloFactory();
        }

        public async Task<TrelloTask> GetTaskById(string userToken, string taskId)
        {
            var task = new Card(taskId, new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            });
            await task.Refresh();

            return ToTrelloTask(task);
        }

        public async Task<TrelloTask[]> GetInactiveTasks(string userToken) =>
            await GetAllColumnTasks(userToken, ColumnTypes.Inactive);

        public async Task<TrelloTask[]> GetActiveTasks(string userToken) =>
            await GetAllColumnTasks(userToken, ColumnTypes.Active);

        public async Task<TrelloTask[]> GetResolvedTasks(string userToken) =>
            await GetAllColumnTasks(userToken, ColumnTypes.Resolved);

        private async Task<TrelloTask[]> GetAllColumnTasks(string userToken, ColumnTypes column)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);

            var boardList = column switch
            {
                ColumnTypes.Inactive => factory.List(boardInfo.InactiveListId, auth),
                ColumnTypes.Active => factory.List(boardInfo.ActiveListId, auth),
                ColumnTypes.Resolved => factory.List(boardInfo.ResolvedListId, auth),
            };

            await boardList.Refresh();

            return boardList.Cards.Select(ToTrelloTask).ToArray();
        }

        public async Task SetAsActive(string userToken, TrelloTask trelloTask)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);
            var card = new Card(trelloTask.Id, auth);

            await card.Refresh();

            var activeList = new List(boardInfo.ActiveListId, auth);
            await activeList.Refresh();

            await activeList.Cards.Add(card);
            await card.Delete();
        }

        public async Task  SetAsDone(string userToken, TrelloTask trelloTask)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);
            var card = new Card(trelloTask.Id, auth);

            await card.Refresh();

            var activeList = new List(boardInfo.ActiveListId, auth);
            await activeList.Refresh();

            await activeList.Cards.Add(card);
            await card.Delete();
        }

        public async Task<TrelloTask> AddNewTask(string userToken, TrelloTask trelloTask)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);
            var column = new List(boardInfo.InactiveListId, auth);

            await column.Refresh();
            var card = await column.Cards.Add(name: trelloTask.Name, description: trelloTask.Description);

            return ToTrelloTask(card);
        }

        private async Task<BoardInfo> GetBoardInfo(string userToken)
        {
            if (userTokenToBoardInfo.TryGetValue(userToken, out var boardInfo))
                return boardInfo;

            TrelloAuthorization.Default.AppKey =
                appKey; // it will not work with multithreading. https://github.com/gregsdennis/Manatee.TrelloAuthorizationProvider/issues/313
            TrelloAuthorization.Default.UserToken = userToken;

            var me = await factory.Me();
            await me.Refresh();
            var board = me.Boards.FirstOrDefault(x => x.Name == "TrelloTaskManager");

            if (board == null)
                throw new Exception();
            await board.Lists.Refresh();
            boardInfo = new BoardInfo
            {
                InactiveListId = board.Lists[0].Id,
                ActiveListId = board.Lists[1].Id,
                ResolvedListId = board.Lists[2].Id,
            };

            userTokenToBoardInfo.Add(userToken, boardInfo);

            return boardInfo;
        }

        private TrelloTask ToTrelloTask(ICard card)
        {
            return new TrelloTask
            {
                Name = card.Name,
                Description = card.Description,
                Id = card.Id,
                Url = card.ShortUrl,
                Key = card.ShortId.Value
            };
        }

        private class BoardInfo
        {
            public string InactiveListId { get; set; }
            public string ActiveListId { get; set; }
            public string ResolvedListId { get; set; }
        }

        private enum ColumnTypes
        {
            Inactive = 0,
            Active = 1,
            Resolved = 2
        }
    }
}