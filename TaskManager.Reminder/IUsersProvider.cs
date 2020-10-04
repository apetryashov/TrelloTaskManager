using System.Collections.Generic;

namespace TaskManager.Reminder
{
    public interface IUsersProvider
    {
        public IAsyncEnumerable<UserWithBoards> GetUsers();
    }
}