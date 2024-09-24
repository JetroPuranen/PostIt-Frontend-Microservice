using PostIt.Application.Dto;

public interface ILoginService
{
    Task<(string Token, string UserId)> LoginUserAsync(LoginDto loginDto); 
}
