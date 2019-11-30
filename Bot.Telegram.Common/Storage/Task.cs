namespace Bot.Telegram.Common.Storage
{
    public class Task : IHasKey<long>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Key { get; set; }
        public override string ToString()
        {
            return @$"
Задача успешно добавлена!
[Название]: {Name}
[Описание]: {Description}

Найти задачу вы всегда можете вызвав комманду /task{Id}
Так же вы можете найти ее на trello доске по ссылке (тут должна быть ссылка)";
        }
    }
}