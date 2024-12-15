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

            if(response == null)
            {
                return null;
            }
            
            // Read and deserialize the response content
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Posts>(jsonResponse);

            return post;
        }
        public async Task<List<Posts>> GetPostsByUserIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync(_dbUrl + "getPostsByUser/" + id);

            if (response == null || !response.IsSuccessStatusCode)
            {
                return new List<Posts>();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Check if the response is empty or invalid JSON
            if (string.IsNullOrWhiteSpace(jsonResponse) || jsonResponse == "NaN")
            {
                return new List<Posts>();
            }

            try
            {
                // Deserialize into a list of posts
                var posts = JsonConvert.DeserializeObject<List<Posts>>(jsonResponse);
                return posts ?? new List<Posts>();
            }
            catch (JsonReaderException)
            {
                return new List<Posts>(); // Return an empty list on deserialization error
            }
        }
    }
}
