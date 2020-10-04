using System.Threading.Tasks;
using TelegramBot.Core.Model;

namespace TaskManager.Reminder
{
    public interface IUserRemindResponseGenerator
    {
        public Task<IResponse> GetResponse(UserWithBoards userWithBoards);
    }
}