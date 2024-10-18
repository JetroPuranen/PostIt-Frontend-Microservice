using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AddPost([FromForm] AddPostDto postDto, IFormFile postPicture)
        {
            if (postDto == null)
            {
                return BadRequest("Post data is null.");
            }

            await _postService.AddPostAsync(postDto, postPicture);
            return Ok("Post created successfully.");
        }
        [HttpGet("getpost/{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            if(id == null)
            {
                return BadRequest("Id is null");
            }
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);
        }
        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (id == null)
            {
                return BadRequest("Id is null");
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Check if the profile picture exists
            string profilePictureUrl = null;
            if (user.ProfilePictureBytes != null)
            {
                // Convert the profile picture to a downloadable file URL (assuming this endpoint serves it)
                profilePictureUrl = Url.Action(nameof(DownloadProfilePicture), new { id = id });
            }

            // Return JSON data along with the picture URL
            return Ok(new
            {
                user.UserId,
                user.Username,
                user.FirstName,
                user.SurName,
                user.EmailAddress,
                user.HomeAddress,
                user.BirthDay,
                ProfilePictureUrl = profilePictureUrl // Include the picture URL
            });
        }

        [HttpGet("downloadProfilePicture/{id}")]
        public async Task<IActionResult> DownloadProfilePicture(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null || user.ProfilePictureBytes == null)
            {
                return NotFound("User or profile picture not found");
            }

            // Return the profile picture as a file download
            return new FileContentResult(user.ProfilePictureBytes, "image/jpeg")
            {
                FileDownloadName = $"{user.Username}_profile.jpg"
            };
        }
        [HttpGet("getPostsByUser/{userId}")]
        public async Task<IActionResult> GetPostsByUser(Guid userId)
        {
            var posts = await _postService.GetPostsByUserIdAsync(userId);

            if (posts == null)
            {
                return NotFound($"No posts found for user with ID {userId}");
            }

            // Return JSON data (including ImageData as base64 string)
            return Ok(posts);
        }
        [HttpGet("getPostImage/{postId}")]
        public async Task<IActionResult> GetPostImage(Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null || post.ImageData == null || post.ImageData.Length == 0)
            {
                return NotFound($"Image for post with ID {postId} not found.");
            }

            // Return the image as a downloadable file (or displayable image)
            return new FileContentResult(post.ImageData, "image/jpeg")
            {
                FileDownloadName = $"post_{postId}_image.jpg"
            };
        }
    }
}

