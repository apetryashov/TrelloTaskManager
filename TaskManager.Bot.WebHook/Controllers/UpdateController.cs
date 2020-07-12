using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Bot.WebHook.Services;
using Telegram.Bot.Types;

namespace TaskManager.Bot.WebHook.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly IUpdateService updateService;

        public UpdateController(IUpdateService updateService) => this.updateService = updateService;

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await updateService.EchoAsync(update);
            return Ok();
        }
    }
}