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
            
            var unfollowData = new UnfollowData
            {
                UserId = unfollowDto.UserId,
                UnfollowUserId = unfollowDto.UnfollowUserId
                
            };

            return await _userRepository.RemoveFollowerFromDatabase(unfollowData);
        }
    }
}
