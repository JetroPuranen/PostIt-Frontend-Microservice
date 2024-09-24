using PostIt.Domain.Data;

namespace PostIt.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUserToDatabase(Users users);
        Task<string> LoginUserInDatabase(string username, string hashedPassword); // Updated to return string
        Task<bool> AddFollowerToDatabase(FollowerData followerDto);
        Task<bool> RemoveFollowerFromDatabase(UnfollowData unfollowDto);
    }

}
