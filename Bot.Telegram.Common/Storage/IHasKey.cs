
namespace Bot.Telegram.Common.Storage
{
    public interface IHasKey<TKey>
    {
        TKey Key { get; set; }
    }
}