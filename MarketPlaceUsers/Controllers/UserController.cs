using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceUsers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = "Test")]
        public IActionResult Test()
        {
            try
            {
                _userService.Teste();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
