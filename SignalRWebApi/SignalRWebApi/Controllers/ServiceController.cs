using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRWebApi.ChatModels;
using SignalRWebApi.ChatServices;

namespace SignalRWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ChatService _chatService;
        public ServiceController(ChatService chatService)
        {
            _chatService = chatService;     
        }

       // [HttpPost]
       [HttpPost("register-user")]
        public IActionResult ResgisterUser([FromBody]UserDTO userDTO)
        {
            if (_chatService.AddUserToList(userDTO.Name))
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Name is already exist, try another name");
            }
        }
    }
}
