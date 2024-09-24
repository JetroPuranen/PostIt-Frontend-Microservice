using PostIt.Domain.Data;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using PostIt.Domain.Interfaces;

namespace PostIt.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbUrl;

        public UserRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _dbUrl = configuration["Database:Url"]; 
        }

        public async Task<bool> AddUserToDatabase(Users user)
        {
            var jsonData = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_dbUrl, content); 
            return response.IsSuccessStatusCode;
        }
        public async Task<string> LoginUserInDatabase(string username, string hashedPassword)
        {
            var loginData = new
            {
                Username = username,
                Password = hashedPassword
            };

            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var loginurl = _dbUrl + "login";
            var response = await _httpClient.PostAsync(loginurl, content);
            var responseContent = await response.Content.ReadAsStringAsync(); 

            Console.WriteLine(responseContent); 

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<Users>(responseContent);
                return responseObject?.UserId; 
            }

            return null; 
        }

        public async Task<bool> AddFollowerToDatabase(FollowerData follower)
        {
           
            var jsonData = JsonConvert.SerializeObject(follower);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_dbUrl}addFollower", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFollowerFromDatabase(UnfollowData unfollowDto)
        {
            var jsonData = JsonConvert.SerializeObject(unfollowDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_dbUrl}unfollowUser", content);
            return response.IsSuccessStatusCode;
        }
    }
    
}
