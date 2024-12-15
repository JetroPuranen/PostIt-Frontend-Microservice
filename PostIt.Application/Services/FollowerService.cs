using PostIt.Application.Interfaces;
using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using PostIt.Application.Dto;

namespace PostIt.Application.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly IUserRepository _userRepository;

        public FollowerService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddFollowerAsync(FollowerDto followerDto)
        {
            var followerData = new FollowerData
            {
                UserId = followerDto.UserId,
                FollowerUserId = followerDto.FollowerUserId
            };

            
            return await _userRepository.AddFollowerToDatabase(followerData);
        }
    }
}
