namespace Bot.Telegram.Common.Storage
{
    public interface ITaskProvider
    {
        Task GetTaskById(long author, int id);
        Task[] GetAllTasks(long author);
        Task[] GetInactiveTasks(long author);
        Task[] GetActiveTasks(long author);
        Task[] GetResolvedTasks(long author);
        void SetAsActive(long author, int taskId);
        void SetAsDone(long author, int taskId);
        void AddNewTask(long author, Task task);
    }
}