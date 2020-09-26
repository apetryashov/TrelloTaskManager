using System;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using TaskManager.Common;
using TaskManager.Common.Tasks;

namespace TaskManager.Trello
{
    public class TrelloTasksHandler : ITaskHandler, ITextButtonMenuProvider // refactor this
    {
        private readonly string appKey;
        private readonly ITrelloFactory factory;
        private readonly IUserItemsStorage<TrelloApiToken> userTokenStorage;

        public TrelloTasksHandler(
            AppKey appKey,
            ITrelloFactory trelloFactory,
            IUserItemsStorage<TrelloApiToken> userTokenStorage
        )
        {
            this.appKey = appKey.Key;
            factory = trelloFactory;
            this.userTokenStorage = userTokenStorage;
        }

        public async Task<MyTask> GetTaskById(long userId, string taskId)
        {
            var task = new Card(taskId, GetUserAuthorization(userId));
            await task.Refresh();
            await task.List.Refresh();

            return ToTrelloTask(task); //need set correct status
        }

        public async Task<MyTask[]> GetAllTasks(long userId, string columnName)
            => await GetAllColumnTasks(userId, columnName);

        public async Task ChangeTaskColumn(long userId, string taskId, string targetColumnId)
        {
            var auth = GetUserAuthorization(userId);
            var allBoardColumnsInfo = await GetAllBoardColumnsInfo(userId);

            var card = new Card(taskId, auth);

            await card.Refresh();

            var boardColumnInfo = allBoardColumnsInfo.FirstOrDefault(x => x.Id == targetColumnId);

            if (boardColumnInfo == default)
                throw new Exception(); //TODO:!

            var list = new List(boardColumnInfo.Id, auth);
            await list.Refresh();
            await card.Move(list.Cards.Count() + 1, list);

            await TrelloProcessor.Flush();
        }

        public async Task<MyTask> AddNewTask(long userId, MyTask myTask)
        {
            var auth = GetUserAuthorization(userId);
            var boardColumnInfo = (await GetAllBoardColumnsInfo(userId)).First();

            var column = new List(boardColumnInfo.Id, auth);
            await column.Refresh();
            var card = await column.Cards.Add(myTask.Name, description: myTask.Description);
            await card.Refresh();

            return ToTrelloTask(card);
        }

        //TODO: сейчас доски ищутся в двух местах. Нужно вынести в одно  пользоваться там!
        public async Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(long userId)
        {
            var trelloAuthorization = GetUserAuthorization(userId);
            return await GetAllBoardColumnsInfo(trelloAuthorization.UserToken);
        }

        public async Task<string[]> GetButtons(long userId) => (await GetAllBoardColumnsInfo(userId))
            .Select(info => info.Name)
            .ToArray();

        private async Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(string userToken)
        {
            var me = await factory.Me(appKey, userToken);
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

        private async Task<MyTask[]> GetAllColumnTasks(long userId, string columnName)
        {
            var auth = GetUserAuthorization(userId);
            var allBoardColumnsInfo = await GetAllBoardColumnsInfo(userId);
            var boardColumnInfo = allBoardColumnsInfo.FirstOrDefault(x => x.Name == columnName);

            if (boardColumnInfo == default)
                throw new Exception(); //TODO:!

            var boardList = factory.List(boardColumnInfo.Id, auth);

            await boardList.Refresh();

            return boardList.Cards.Select(ToTrelloTask).ToArray();
        }

        private TrelloAuthorization GetUserAuthorization(long userId)
        {
            var token = userTokenStorage.Get(userId);

            if (token == null)
                throw new ArgumentException($"token for user with id {userId} not found");

            return new TrelloAuthorization
            {
                AppKey = appKey,
                UserToken = token.Token
            };
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