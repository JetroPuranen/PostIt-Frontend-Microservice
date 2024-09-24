using PostIt.Application.Interfaces;
using PostIt.Domain.Interfaces;
using PostIt.Application.Dto;
using PostIt.Domain.Data;

namespace PostIt.Application.Services
{
    public class UnfollowService : IUnfollowService
    {
        private readonly IUserRepository _userRepository;

        public UnfollowService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RemoveFollowerAsync(UnfollowDto unfollowDto)
        {
            if (string.IsNullOrEmpty(unfollowDto.Username) || string.IsNullOrEmpty(unfollowDto.UnfollowUsername))
            {
                throw new ArgumentException("Invalid unfollow data.");
            }

            var unfollowData = new UnfollowData
            {
                UnfollowUsername = unfollowDto.Username,
                Username = unfollowDto.Username,
            };

            return await _userRepository.RemoveFollowerFromDatabase(unfollowData);
        }
    }
}
