namespace Bot.Telegram.Common.Storage
{
    public class Task : IHasKey<long>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Key { get; set; }
    }
}