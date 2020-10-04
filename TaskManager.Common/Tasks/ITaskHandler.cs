using System.Threading.Tasks;

namespace TaskManager.Common.Tasks
{
    public interface ITaskHandler
    {
        Task<MyTask> GetTaskById(long userId, string taskId);
        Task<MyTask> GetTaskById(string userToken, string taskId);
        Task<MyTask[]> GetAllColumnTasks(long userId, string columnName);
        Task<MyTask[]> GetAllColumnTasks(string userToken, string columnName);
        Task ChangeTaskColumn(long userId, string taskId, string targetColumnId);
        Task<MyTask> AddNewTask(long userId, MyTask task);
        Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(long userId);
        Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(string userToken);
    }
}