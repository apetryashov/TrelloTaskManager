using System.Threading.Tasks;

namespace TaskManager.Common
{
    public interface ITextButtonMenuProvider
    {
        public Task<string[]> GetButtons(long userId);
    }
}