using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Ether_bot.Interfaces;

namespace Ether_bot.Controllers
{
    [ApiController]
    [Route("/")]
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