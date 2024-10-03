using PostIt.Domain.Data;

namespace PostIt.Domain.Interfaces
{
    public interface IPostsRepository
    {
        Task AddAsync(Posts post);
        Task<Posts> GetAsync(Guid id);
    }
}
