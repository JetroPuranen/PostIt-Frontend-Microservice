public interface ILoginService
{
    Task<(string Token, string UserId)> LoginUserAsync(string username, string password);
}