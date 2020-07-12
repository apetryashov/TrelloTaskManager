using System;

namespace TaskManager.Common.Tasks
{
    public static class TaskStatusExtension
    {
        public static string AsPrintableString(this TaskStatus status) => status switch
        {
            TaskStatus.Resolved => "Сделана",
            TaskStatus.Active => "Активна",
            TaskStatus.Inactive => "Неактивна",
            _ => throw new ArgumentException($"unsupported task status {status}")
        };
    }
}