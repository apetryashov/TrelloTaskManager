using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using TaskManager.Common.Tasks;
using TaskStatus = TaskManager.Common.Tasks.TaskStatus;

namespace TaskManager.Trello
{
    public class TrelloTasksHandler : ITaskHandler // refactor this
    {
        private readonly string appKey;
        private readonly ITrelloFactory factory;
        private readonly Dictionary<string, BoardInfo> userTokenToBoardInfo = new Dictionary<string, BoardInfo>();

        public TrelloTasksHandler(string appKey)
        {
            this.appKey = appKey;
            factory = new TrelloFactory();
        }

        public async Task<MyTask> GetTaskById(string userToken, string taskId)
        {
            var task = new Card(taskId, new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            });
            await task.Refresh();

            return ToTrelloTask(task, TaskStatus.Inactive); //need set correct status
        }

        public async Task<MyTask[]> GetAllTasks(string userToken, TaskStatus status)
        {
            return await GetAllColumnTasks(userToken, status);
        }

        public async Task ChangeTaskStatus(string userToken, MyTask task, TaskStatus toTaskStatus)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);
            var card = new Card(task.Id, auth);

            await card.Refresh();

            var list = toTaskStatus switch
            {
                TaskStatus.Active => new List(boardInfo.ActiveListId, auth),
                TaskStatus.Inactive => new List(boardInfo.InactiveListId, auth),
                TaskStatus.Resolved => new List(boardInfo.ResolvedListId, auth),
            };

            await list.Refresh();

            await list.Cards.Add(card);
            await card.Delete();
        }

        private async Task<MyTask[]> GetAllColumnTasks(string userToken, TaskStatus status)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);

            var boardList = status switch
            {
                TaskStatus.Inactive => factory.List(boardInfo.InactiveListId, auth),
                TaskStatus.Active => factory.List(boardInfo.ActiveListId, auth),
                TaskStatus.Resolved => factory.List(boardInfo.ResolvedListId, auth),
            };

            await boardList.Refresh();

            return boardList.Cards.Select(card => ToTrelloTask(card, status)).ToArray();
        }

        public async Task<MyTask> AddNewTask(string userToken, MyTask myTask)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardInfo = await GetBoardInfo(userToken);
            var column = new List(boardInfo.InactiveListId, auth);

            await column.Refresh();
            var card = await column.Cards.Add(name: myTask.Name, description: myTask.Description);

            return ToTrelloTask(card, TaskStatus.Inactive);
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

        private MyTask ToTrelloTask(ICard card, TaskStatus status)
        {
            return new MyTask
            {
                Name = card.Name,
                Description = card.Description,
                Id = card.Id,
                Url = card.ShortUrl,
                Key = card.ShortId.Value,
                TaskStatus = status
            };
        }

        private class BoardInfo
        {
            public string InactiveListId { get; set; }
            public string ActiveListId { get; set; }
            public string ResolvedListId { get; set; }
        }
    }
}