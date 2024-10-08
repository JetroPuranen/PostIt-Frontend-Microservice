﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Application.Dto;
using PostIt.Application.Interfaces;


namespace PostIt.FrontEndMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontEndController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IFollowerService _followerService;
        private readonly IUnfollowService _unfollowService;
        private readonly IPostService _postService;
        private readonly IConfiguration _configuration;
        
        public FrontEndController(
            IUserService userService,
            ILoginService loginService,
            IFollowerService followerService,
            IUnfollowService unfollowService, IPostService postService,
            IConfiguration configuration)
        {
            _userService = userService;
            _loginService = loginService;
            _followerService = followerService;
            _unfollowService = unfollowService;
            _configuration = configuration;
            _postService = postService;
        }

        [HttpPost("addUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromForm] CreateUserDto createUserDto, IFormFile profilePicture)
        {
            if (createUserDto == null)
            {
                return BadRequest("User data is null.");
            }


            var validationResult = _userService.ValidateUser(createUserDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Message);
            }

            var result = await _userService.AddUserAsync(createUserDto, profilePicture);
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

            var (token, userId) = await _loginService.LoginUserAsync(loginDto);

            if (token != null && userId != null)
            {
                return Ok(new { Token = token, UserId = userId });
            }

            return Unauthorized("Invalid username or password.");
        }

        [HttpPost("addFollower")]
        [Authorize]
        public async Task<IActionResult> AddFollower([FromForm] FollowerDto followerDto)
        {
            if (followerDto == null)
            {
                return BadRequest("Follower data is null.");
            }


            var result = await _followerService.AddFollowerAsync(followerDto);
            if (result)
            {
                return Ok("Follower added successfully.");
            }

            return StatusCode(500, "An error occurred while adding the follower.");
        }

        [HttpPost("unfollowUser")]
        [Authorize]
        public async Task<IActionResult> UnfollowUser([FromForm] UnfollowDto unfollowDto)
        {
            if (unfollowDto == null)
            {
                return BadRequest("Unfollow data is null.");
            }

            var result = await _unfollowService.RemoveFollowerAsync(unfollowDto);
            if (result)
            {
                return Ok("Unfollowed user successfully.");
            }

            return StatusCode(500, "An error occurred while unfollowing the user.");
        }
        [HttpPost("addPost")]
        public async Task<IActionResult> AddPost([FromForm] PostDto postDto, IFormFile postPicture)
        {
            if (postDto == null)
            {
                return BadRequest("Post data is null.");
            }

            await _postService.AddPostAsync(postDto, postPicture);
            return Ok("Post created successfully.");
        }
    }
}

