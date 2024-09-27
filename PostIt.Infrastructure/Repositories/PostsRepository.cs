using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using System.Text;


namespace PostIt.Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbUrl;
        public PostsRepository(HttpClient httpClient, IConfiguration configuration) 
        {
          _httpClient = httpClient;
          _dbUrl = configuration["Database:Url"];
        }
        public async Task AddAsync(Posts post)
        {
            var jsonData = JsonConvert.SerializeObject(post);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_dbUrl + "addPost", content);
            
        }
    }
}
