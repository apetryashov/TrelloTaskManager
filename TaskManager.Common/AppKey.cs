using TaskManager.Common.Storage;

namespace TaskManager.Common
{
    public class AppKey : IHasKey<string>
    {
        public string Key { get; set; }
    }
}