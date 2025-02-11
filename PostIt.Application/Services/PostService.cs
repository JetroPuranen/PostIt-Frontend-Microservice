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
            byte[] imageData = null; 

            if (image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();  
                }
            }
            else
            {
                _logger.LogWarning("Image not provided");
            }

            
            var post = new Posts
            {
                UserId = postDto.UserId,
                Caption = postDto.Caption,
                ImageData = imageData 
            };

            // Call repository to save the post
            await _postsRepository.AddAsync(post);
        }


        public async Task<PostDto> GetPostByIdAsync(Guid id)
        {
            var post = await _postsRepository.GetAsync(id);

            return new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Caption = post.Caption,
                ImageData = post.ImageData,
                Comments = post.Comments?.Select(c => new CommentDto
                {
                    UserId = c.UserId,
                    Comment = c.Comment
                }).ToList() ?? new List<CommentDto>(),

                LikeCount = post.LikeCount,
                WhoHasLiked = post.WhoHasLiked?.ToList() ?? new List<Guid>(), // Muunnetaan List<Guid> -muotoon
                WhoHasCommented = post.Comments?.Select(c => c.UserId).ToList() ?? new List<Guid>(),
                CreatedAt = post.CreatedAt,
            };
        }

        public async Task<List<PostDto>> GetPostsByUserIdAsync(Guid id)
        {
            var posts = await _postsRepository.GetPostsByUserIdAsync(id);

            return posts.Select(post => new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Caption = post.Caption,
                ImageData = post.ImageData,
                Comments = post.Comments?.Select(c => new CommentDto
                {
                    UserId = c.UserId,
                    Comment = c.Comment
                }).ToList() ?? new List<CommentDto>(),

                LikeCount = post.WhoHasLiked?.Count ?? 0,
                WhoHasLiked = post.WhoHasLiked?.ToList() ?? new List<Guid>(), // Muunnetaan List<Guid> -muotoon
                WhoHasCommented = post.Comments?.Select(c => c.UserId).ToList() ?? new List<Guid>(),
                CreatedAt = post.CreatedAt,
            }).ToList();
        }


        public async Task<bool> UpdatePostAsync(UpdatePostDto updatePostDto)
        {
            if (updatePostDto.Id == null)
            {
                _logger.LogError("Post ID cannot be null");
                throw new ArgumentException("Post ID is required");
            }

            var updatePost = new UpdatePost
            {
                Id = updatePostDto.Id,
                Caption = updatePostDto.Caption,
                LikedByUserId = updatePostDto.LikedByUserId,
                Comment = updatePostDto.Comment != null
                    ? new PostComment
                    {
                        UserId = updatePostDto.Comment.UserId ?? Guid.Empty,  
                        Comment = updatePostDto.Comment.Comment
                    }
                    : null 
            };

            return await _postsRepository.UpdateAsync(updatePost);
           

        }
    }
}
