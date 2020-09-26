using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Bot.Commands.Authorization;
using Telegram.Bot;
using TelegramBot.Core;

namespace TaskManager.Bot.WebHook.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthorizationResponseCommand authorizationResponseCommand;
        private readonly ITelegramBotClient client;

        public AuthController(ITelegramBotClient client, AuthorizationResponseCommand authorizationResponseCommand)
        {
            this.client = client;
            this.authorizationResponseCommand = authorizationResponseCommand;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromQuery(Name = "telegram_id")] long telegramId,
            [FromQuery] string token
        )
        {
            var response = await authorizationResponseCommand.StartCommand(telegramId, token);
            await client.SendResponse(telegramId, response);

            return Ok();
        }
    }
}