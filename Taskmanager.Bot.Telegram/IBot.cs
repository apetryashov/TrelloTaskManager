using System;
using TaskManager.Bot.Telegram.Model;

namespace TaskManager.Bot.Telegram
{
    public interface IBot
    {
        event Action<IRequest> OnRequest;
        event Action<Exception> OnError;
        void Start();
        void Stop();
    }
}