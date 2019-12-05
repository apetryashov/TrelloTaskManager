using TaskManager.Common.Storage;

namespace TaskManager.Common
{
    //extract
    public class AppKey : IHasKey<string>
    {
        public string Key { get; set; }
    }
}