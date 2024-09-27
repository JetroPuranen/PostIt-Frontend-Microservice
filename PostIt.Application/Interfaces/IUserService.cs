using Microsoft.AspNetCore.Http;
using PostIt.Application.Dto;


namespace PostIt.Application.Interfaces
{
    public interface IUserService
    {
        (bool IsValid, string Message) ValidateUser(CreateUserDto userDto);
        Task<bool> AddUserAsync(CreateUserDto userDto, IFormFile profilepicture);

        
    }
}
