using Microsoft.AspNetCore.Http;
using PostIt.Application.Dto;


namespace PostIt.Application.Interfaces
{
    public interface IPostService
    {
        Task AddPostAsync(PostDto postDto, IFormFile image);
        Task<PostDto> GetPostByIdAsync(Guid id);
    }
}
