using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                if (users == null)
                {
                    return NotFound();
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("AllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _userService.GetAllMessages();
                if (messages == null)
                {
                    return NotFound();
                }
                return Ok(messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            try
            {
                var user = await _userService.GetUser(id);
                if (user != null)
                    return Ok(user);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("Message/{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            try
            {
                var message = await _userService.GetMessage(id);
                if (message != null)
                    return Ok(message);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpPost("WriteMessage")]
        public async Task<IActionResult> WriteMessage([FromBody] Message Message)
        {
            try
            {
                if(await _userService.WriteMessage(Message))
                    return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] User user)
        {
            try
            {
                if(await _userService.UpdateUser(id, user))
                    return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpPut("UpdateMessage/{id}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] Guid id, Message Message)
        {
            try
            {
                if(await _userService.UpdateMessage(id, Message))
                    return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            try
            {
                if (await _userService.DeleteUser(id))
                    return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteMessage/{id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
        {
            try
            {
                if(await _userService.DeleteMessage(id))
                    return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
    }
}
