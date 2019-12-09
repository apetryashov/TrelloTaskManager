using System;
using TaskManager.Bot.Model;

namespace TaskManager.Bot
{
    public interface IBot
    {
        event Action<IRequest> OnRequest;
        event Action<Exception> OnError;
        void Start();
        void Stop();
    }
}