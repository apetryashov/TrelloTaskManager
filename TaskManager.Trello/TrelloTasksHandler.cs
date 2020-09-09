using System;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using TaskManager.Common;
using TaskManager.Common.Tasks;

namespace TaskManager.Trello
{
    public class TrelloTasksHandler : ITaskHandler // refactor this
    {
        private readonly string appKey;
        private readonly ITrelloFactory factory;

        public TrelloTasksHandler(AppKey appKey, ITrelloFactory trelloFactory)
        {
            this.appKey = appKey.Key;
            factory = trelloFactory;
        }

        public async Task<MyTask> GetTaskById(string userToken, string taskId)
        {
            var task = new Card(taskId, new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            });
            await task.Refresh();
            await task.List.Refresh();

            return ToTrelloTask(task); //need set correct status
        }

        public async Task<MyTask[]> GetAllTasks(string userToken, string columnName)
            => await GetAllColumnTasks(userToken, columnName);

        public async Task ChangeTaskColumn(string userToken, MyTask task, string targetColumnId)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var allBoardColumnsInfo = await GetAllBoardColumnsInfo(userToken);

            var card = new Card(task.Id, auth);

            await card.Refresh();

            var boardColumnInfo = allBoardColumnsInfo.FirstOrDefault(x => x.Id == targetColumnId);

            if (boardColumnInfo == default)
                throw new Exception(); //TODO:!

            var list = new List(boardColumnInfo.Id, auth);
            await list.Refresh();
            await card.Move(list.Cards.Count() + 1, list);

            await TrelloProcessor.Flush();
        }

        public async Task<MyTask> AddNewTask(string userToken, MyTask myTask)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var boardColumnInfo = (await GetAllBoardColumnsInfo(userToken)).First();

            var column = new List(boardColumnInfo.Id, auth);
            await column.Refresh();
            var card = await column.Cards.Add(myTask.Name, description: myTask.Description);
            await card.Refresh();

            return ToTrelloTask(card);
        }

        private async Task<MyTask[]> GetAllColumnTasks(string userToken, string columnName)
        {
            var auth = new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = userToken
            };
            var allBoardColumnsInfo = await GetAllBoardColumnsInfo(userToken);
            var boardColumnInfo = allBoardColumnsInfo.FirstOrDefault(x => x.Name == columnName);

            if (boardColumnInfo == default)
                throw new Exception(); //TODO:!

            var boardList = factory.List(boardColumnInfo.Id, auth);

            await boardList.Refresh();

            return boardList.Cards.Select(ToTrelloTask).ToArray();
        }

        //TODO: сейчас доски ищутся в двух местах. Нужно вынести в одно  пользоваться там!
        public async Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(string userToken)
        {
            TrelloAuthorization.Default.AppKey =
                appKey; // it will not work with multithreading. https://github.com/gregsdennis/Manatee.TrelloAuthorizationProvider/issues/313
            TrelloAuthorization.Default.UserToken = userToken;

            var me = await factory.Me();
            await me.Refresh();
            var board = me.Boards.FirstOrDefault(x => x.Name == "TrelloTaskManager");

            if (board == null)
                throw new Exception(); //TODO:
            await board.Lists.Refresh();

            return board.Lists
                .Select(column => new BoardColumnInfo
                {
                    Id = column.Id,
                    Name = column.Name
                })
                .ToArray();
        }

        private static MyTask ToTrelloTask(ICard card) => new MyTask
        {
            Name = card.Name,
            Description = card.Description,
            Id = card.Id,
            Url = card.ShortUrl,
            Status = card.List.Name
        };
    }
}
//170!