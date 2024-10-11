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

            var response = await _httpClient.PostAsync(_dbUrl + "addPost/", content);
            
        }

        public async Task<Posts> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync(_dbUrl + "getPost/" + id);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Read and deserialize the response content
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Posts>(jsonResponse);

            return post;
        }
        public async Task<List<Posts>> GetPostsByUserIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync(_dbUrl + "getPostsByUser/" + id);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize into a list of posts
            var posts = JsonConvert.DeserializeObject<List<Posts>>(jsonResponse);

            return posts;
        }
    }
}
