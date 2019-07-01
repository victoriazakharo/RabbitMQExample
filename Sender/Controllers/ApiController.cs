using Microsoft.AspNetCore.Mvc;

namespace Sender.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private MessageSender sender;

        public ApiController(MessageSender messageSender)
        {
            sender = messageSender;
        }

        [HttpGet]
        [Route("send/{message}")]
        public ActionResult<string> Get(string message)
        {
            return sender.SendMessage(message) ? $"Sent '{message}'": "Waiting for RabbitMq server to start";
        }
    }
}
