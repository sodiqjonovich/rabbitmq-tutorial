using Microsoft.AspNetCore.Mvc;
using SenderApp.Api.Interfaces;
using SenderApp.Api.Models;

namespace SenderApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IProducerService _producer;
        public MessagesController(IProducerService producerService)
        {
            this._producer = producerService;
        }

        [HttpPost]
        public IActionResult SendMessage([FromForm] Message message)
        {
            _producer.Send(message);
            return Ok();
        }
    } 
}
