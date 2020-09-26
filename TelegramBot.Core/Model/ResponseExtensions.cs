using System.Threading.Tasks;

namespace TelegramBot.Core.Model
{
    public static class ResponseExtensions
    {
        public static Task<IResponse> RunAsTask(this IResponse response) => Task.Run(() => response);
    }
}