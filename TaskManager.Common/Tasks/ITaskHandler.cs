using System.Threading.Tasks;

namespace TaskManager.Common.Tasks
{
    public interface ITaskHandler
    {
        Task<MyTask> GetTaskById(string userToken, string taskId);
        Task<MyTask[]> GetAllTasks(string userToken, string columnName);
        Task ChangeTaskColumn(string userToken, MyTask task, string targetColumnId);
        Task<MyTask> AddNewTask(string userToken, MyTask task);
        Task<BoardColumnInfo[]> GetAllBoardColumnsInfo(string userToken);
    }
}