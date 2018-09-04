using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Ether_bot.Services;

namespace Ether_bot.Controllers
{
    [Route("/")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        IUpdateService _updateservice;
        public MessageController(IUpdateService updateService)
        {
            _updateservice = updateService;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]Update message)
        {                        
            await _updateservice.SendAnswerAsync(message);
            return Ok();
        }

    }
}