using System.Collections.Generic;
using TaskManager.Common.Tasks;

namespace TaskManager.Reminder
{
    public class UserWithBoards
    {
        public long TelegramId { get; set; }
        public string UserToken { get; set; }
        public IEnumerable<BoardColumnInfo> Boards { get; set; }
    }
}