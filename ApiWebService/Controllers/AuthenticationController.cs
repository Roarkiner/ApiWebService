using ApiWebService.Contracts;
using ApiWebService.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        public AuthenticationController(IJwtAuthenticationService jwtAuthenticationService) 
        {
            _jwtAuthenticationService = jwtAuthenticationService;
        }

        [HttpPost]
        public IActionResult Login()
        {
            var token = _jwtAuthenticationService.GenerateToken();
            return Ok(token);
        }
    }
}
