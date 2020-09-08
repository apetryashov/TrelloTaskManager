using System.Text;
using JetBrains.Annotations;

namespace TaskManager.Common.Tasks
{
    public class MyTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [CanBeNull] public string Description { get; set; }
        public string Url { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Inactive;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[Название]: {Name}");

            if (Description != null)
                sb.AppendLine($"[Описание]: {Description}");

            sb.AppendLine($"[Статус]: {Status.AsPrintableString()}");

            sb.AppendLine($@"

Найти задачу вы всегда можете вызвав комманду /task_{Id}
Так же вы можете найти ее на TrelloTaskManager доске по ссылке {Url}");

            return sb.ToString();
        }
    }
}