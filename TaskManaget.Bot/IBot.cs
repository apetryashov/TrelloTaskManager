using System;
using TaskManaget.Bot.Model;

namespace TaskManaget.Bot
{
    public interface IBot
    {
        event Action<IRequest> OnRequest;
        event Action<Exception> OnError;
        void Start();
        void Stop();
    }
}