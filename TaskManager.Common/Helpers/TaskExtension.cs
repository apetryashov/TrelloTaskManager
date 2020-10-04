using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Common.Helpers
{
    public static class TaskExtension
    {
        public static async IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(this IEnumerable<Task<TItem>> tasks)
        {
            foreach (var task in tasks)
                yield return await task;
        }
    }
}