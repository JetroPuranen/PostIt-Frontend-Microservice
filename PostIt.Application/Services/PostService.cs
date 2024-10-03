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

        public async Task AddPostAsync(PostDto postDto, IFormFile image)
        {
            string imageData = null;

            if (image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    imageData = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                _logger.LogWarning("Image not given");
            }



            var post = new Posts
            {
                UserId = postDto.UserId,
                ImageData = imageData,  
                Caption = postDto.Caption,
                Comments = postDto.Comments,
                LikeCount = postDto.LikeCount,
                WhoHasLiked = postDto.WhoHasLiked
            };

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
    }
}
