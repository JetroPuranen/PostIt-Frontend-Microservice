using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;


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
        public async Task<bool> LoginUserInDatabase(string username, string hashedPassword)
        {
            var loginData = new
            {
                Username = username,
                Password = hashedPassword
            };

            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var loginurl = "https://localhost:7035/api/Database/login";
            var response = await _httpClient.PostAsync(loginurl, content);
            return response.IsSuccessStatusCode;
        }
    }
}
