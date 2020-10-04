using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Common.Tasks;
using TaskManager.Trello;
using TelegramBot.Core.Model;

namespace TaskManager.Reminder
{
    public class UserRemindResponseGenerator : IUserRemindResponseGenerator
    {
        private readonly ITaskHandler taskHandler;

        private const string StartRemindMessage =
            @"Привет! Мы заметили, что ты давно не менял статуст у накоторых задач. Возможно что-то изменилось?
Вот их список:";


        public UserRemindResponseGenerator(ITaskHandler taskHandler) => this.taskHandler = taskHandler;

        public async Task<IResponse> GetResponse(UserWithBoards userWithBoards)
        {
            var taskLinks = await GetTaskLinks(userWithBoards);

            if (!taskLinks.Any())
                return EmptyResponse.Create();

            return InlineButtonResponse.CreateWithVerticalButtons(StartRemindMessage, taskLinks);
        }

        private async Task<(string text, string callback)[]> GetTaskLinks(UserWithBoards userWithBoards)
        {
            var token = userWithBoards.UserToken;
            var links = new List<(string, string)>();

            foreach (var board in userWithBoards.Boards)
            {
                var boardTaskLinks = (await taskHandler.GetAllColumnTasks(token, board.Name))
                    .Select(x => (x.Name, x.GetLinkString()))
                    .ToArray();
                links.AddRange(boardTaskLinks);
            }

            return links.ToArray();
        }
    }
}