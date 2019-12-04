namespace TaskManager.Common.Storage
{
    public interface ITaskProvider
    {
        TrelloTask GetTaskById(long author, int id);
        TrelloTask[] GetAllTasks(long author);
        TrelloTask[] GetInactiveTasks(long author);
        TrelloTask[] GetActiveTasks(long author);
        TrelloTask[] GetResolvedTasks(long author);
        void SetAsActive(long author, int taskId);
        void SetAsDone(long author, int taskId);
        void AddNewTask(long author, TrelloTask trelloTask);
    }
}