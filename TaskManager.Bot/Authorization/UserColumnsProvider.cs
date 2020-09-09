using System.Collections.Generic;
using System.Linq;
using TaskManager.Bot.Model.Domain;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Authorization
{
    //TODO: нужно подумать над объединением с ITaskHandler
    public class UserColumnsProvider : ITextButtonMenuProvider
    {
        private readonly ITaskHandler taskHandler;

        public UserColumnsProvider(ITaskHandler taskHandler) => this.taskHandler = taskHandler;

        public string[] GetButtons(Author author) => GetUserBoardInfo(author)
            .ColumnIdToColumnName
            .Values
            .ToArray();

        private UserBoardInfo GetUserBoardInfo(Author author)
        {
            var telegramId = author.TelegramId;
            var allBoardColumnsInfo = taskHandler.GetAllBoardColumnsInfo(author.UserToken).Result;

            return new UserBoardInfo
            {
                TelegramId = telegramId,
                ColumnIdToColumnName = allBoardColumnsInfo.ToDictionary(
                    x => x.Id,
                    x => x.Name
                )
            };
        }

        private class UserBoardInfo
        {
            public long TelegramId { get; set; }
            public Dictionary<string, string> ColumnIdToColumnName { get; set; }
        }
    }
}