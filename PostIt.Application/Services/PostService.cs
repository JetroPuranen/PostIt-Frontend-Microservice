using Microsoft.AspNetCore.Http;
using PostIt.Application.Dto;
using PostIt.Application.Interfaces;
using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace PostIt.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ILogger<PostService> _logger;
        public PostService(IPostsRepository postRepository)
        {
            _postsRepository = postRepository;
        }

        public async Task AddPostAsync(AddPostDto postDto, IFormFile image)
        {
            byte[] imageData = null; // Store image as byte array

            if (image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();  // Convert image to byte array
                }
            }
            else
            {
                _logger.LogWarning("Image not provided");
            }

            // Create a new post object and set its properties
            var post = new Posts
            {
                UserId = postDto.UserId,
                Caption = postDto.Caption,
                ImageData = imageData // Set image data as byte array
            };

            // Call repository to save the post
            await _postsRepository.AddAsync(post);
        }


        public async Task<PostDto> GetPostByIdAsync(Guid id)
        {
            var post = await _postsRepository.GetAsync(id);

            
            var postDto = new PostDto
            {
                
                Caption = post.Caption,
                Comments = post.Comments,
                LikeCount = post.LikeCount,
                WhoHasLiked = post.WhoHasLiked,
                ImageData = post.ImageData,
            };

            return postDto;
        }

        public async Task<List<PostDto>> GetPostsByUserIdAsync(Guid id)
        {
            var posts = await _postsRepository.GetPostsByUserIdAsync(id); // This will be List<Posts>

            // Convert List<Posts> to List<PostDto>
            var postDtos = posts.Select(post => new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Caption = post.Caption,
                Comments = post.Comments,
                LikeCount = post.LikeCount,
                WhoHasLiked = post.WhoHasLiked,
                
            }).ToList();

            return postDtos;
        }
    }
}
