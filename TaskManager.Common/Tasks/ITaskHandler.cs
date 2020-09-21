using System.Threading.Tasks;

namespace TaskManager.Common.Tasks
{
    public interface ITaskHandler
    {
        Task<MyTask> GetTaskById(long userId, string taskId);
        Task<MyTask[]> GetAllTasks(long userId, string columnName);
        Task ChangeTaskColumn(long userId, string taskId, string targetColumnId);
        Task<MyTask> AddNewTask(long userId, MyTask task);
        Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(long userId);
    }
}