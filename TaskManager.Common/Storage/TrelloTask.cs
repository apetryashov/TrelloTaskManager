using System.Text;
using JetBrains.Annotations;

namespace TaskManager.Common.Storage
{
    public class TrelloTask : IHasKey<long> // rename and extract to a separate project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [CanBeNull] public string Description { get; set; }
        public long Key { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[Название]: {Name}");

            if (Description != null)
                sb.AppendLine($"[Описание]: {Description}");

            sb.AppendLine($@"

Найти задачу вы всегда можете вызвав комманду /task_{Id}
Так же вы можете найти ее на TrelloTaskManager доске по ссылке {Url}");

            return sb.ToString();
        }
    }
}