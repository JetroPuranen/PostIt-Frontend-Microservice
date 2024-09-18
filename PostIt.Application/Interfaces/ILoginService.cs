namespace PostIt.Application.Services
{
    public interface ILoginService
    {
        Task<bool> LoginUserAsync(string username, string password);
    }
}
