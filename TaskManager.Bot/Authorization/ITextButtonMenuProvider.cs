using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Authorization
{
    public interface ITextButtonMenuProvider //TODO: имя не нравится
    {
        public string[] GetButtons(Author author);
    }
}