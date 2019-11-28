using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Common
{
    public class MoqTaskProvider : ITaskProvider
    {
        private readonly Dictionary<int, Task> activeTasks = new Dictionary<int, Task>();
        private readonly Dictionary<int, Task> inactiveTasks = new Dictionary<int, Task>();
        private readonly Dictionary<int, Task> resolvedTasks = new Dictionary<int, Task>();
        private int id = 7;

        public MoqTaskProvider()
        {
            inactiveTasks.Add(1, new Task {Id = 1, Name = "task1", Description = "simple description1"});
            inactiveTasks.Add(2, new Task {Id = 2, Name = "task2", Description = "simple description2"});
            inactiveTasks.Add(3, new Task {Id = 3, Name = "task3", Description = "simple description3"});
            inactiveTasks.Add(4, new Task {Id = 4, Name = "task4", Description = "simple description4"});
            activeTasks.Add(5, new Task {Id = 5, Name = "task5", Description = "simple description5"});
            activeTasks.Add(6, new Task {Id = 6, Name = "task6", Description = "simple description6"});
        }

        public Task GetTaskById(long author, int id)
        {
            if (inactiveTasks.TryGetValue(id, out var task))
                return task;
            if (activeTasks.TryGetValue(id, out task))
                return task;
            if (resolvedTasks.TryGetValue(id, out task))
                return task;

            throw new AggregateException();
        }

        public Task[] GetAllTasks(long author)
        {
            return inactiveTasks.Values
                .Union(activeTasks.Values)
                .Union(resolvedTasks.Values)
                .ToArray();
        }

        public Task[] GetInactiveTasks(long author)
        {
            return inactiveTasks.Values.ToArray();
        }

        public Task[] GetActiveTasks(long author)
        {
            return activeTasks.Values.ToArray();
        }

        public Task[] GetResolvedTasks(long author)
        {
            return resolvedTasks.Values.ToArray();
        }

        public void SetAsActive(long author, int taskId)
        {
            var task = GetTaskById(author, taskId);
            inactiveTasks.Remove(taskId);

            activeTasks.Add(taskId, task);
        }

        public void SetAsDone(long author, int taskId)
        {
            var task = GetTaskById(author, taskId);
            activeTasks.Remove(taskId);

            resolvedTasks.Add(taskId, task);
        }

        public void AddNewTask(long author, Task task)
        {
            task.Id = GetNewId();
            inactiveTasks.Add(task.Id, task);
        }

        private int GetNewId() => id++;
    }
}