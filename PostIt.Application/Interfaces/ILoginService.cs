namespace PostIt.Application.Services
{
    public interface ILoginService
    {
        Task<string> LoginUserAsync(string username, string password);
    }
}
