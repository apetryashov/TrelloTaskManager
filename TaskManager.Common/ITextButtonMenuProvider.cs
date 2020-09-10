using TaskManager.Common.Domain;

namespace TaskManager.Common
{
    public interface ITextButtonMenuProvider
    {
        public string[] GetButtons(Author author);
    }
}