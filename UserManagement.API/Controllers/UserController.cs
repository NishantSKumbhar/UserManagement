using Microsoft.AspNetCore.Mvc;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Success!";
        }
    }
}
