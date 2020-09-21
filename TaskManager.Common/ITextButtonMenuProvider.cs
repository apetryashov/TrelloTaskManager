using TelegramBot.Core.Domain;

namespace TaskManager.Common
{
    public interface ITextButtonMenuProvider
    {
        public string[] GetButtons(long userId);
    }
}