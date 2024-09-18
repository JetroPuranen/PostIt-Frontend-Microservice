
using PostIt.Domain.Data;

namespace PostIt.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUserToDatabase(Users users);
        Task<bool> LoginUserInDatabase(string username, string hashedPassword);
    }
}
