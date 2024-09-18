using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Application.Dto;
using PostIt.Application.Interfaces;
using PostIt.Application.Services;

namespace PostIt.FrontEndMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontEndController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        public FrontEndController(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        [HttpPost("addUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromForm] CreateUserDto createUserDto, IFormFile profilePicture)
        {
            if (createUserDto == null)
            {
                return BadRequest("User data is null.");
            }

            if (profilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    createUserDto.ProfilePicture = Convert.ToBase64String(memoryStream.ToArray()); 
                }
            }

            var validationResult = _userService.ValidateUser(createUserDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Message);
            }

            var result = await _userService.AddUserAsync(createUserDto);
            if (result)
            {
                return Ok("User created and sent to database successfully.");
            }

            return StatusCode(500, "An error occurred while creating the user.");
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromForm] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid login data.");
            }

            var loginResult = await _loginService.LoginUserAsync(loginDto.Username, loginDto.Password);

            if (loginResult)
            {
                return Ok("Login successful.");
            }

            return Unauthorized("Invalid username or password.");
        }
    }

}

