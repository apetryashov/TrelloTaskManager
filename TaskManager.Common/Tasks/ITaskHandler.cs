using System.Threading.Tasks;

namespace TaskManager.Common.Tasks
{
    public interface ITaskHandler
    {
        Task<MyTask> GetTaskById(string userToken, string taskId);
        Task<MyTask[]> GetAllTasks(string userToken, TaskStatus taskType);
        Task ChangeTaskStatus(string userToken, MyTask task, TaskStatus toTaskStatus);
        Task<MyTask> AddNewTask(string userToken, MyTask task);
    }
}