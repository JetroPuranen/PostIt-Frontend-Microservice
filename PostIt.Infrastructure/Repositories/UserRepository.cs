﻿using PostIt.Domain.Data;
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

            var response = await _httpClient.PostAsync(_dbUrl + "addUser", content); 
            return response.IsSuccessStatusCode;
        }
        public async Task<string> LoginUserInDatabase(LoginData loginData)
        {
            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var loginUrl = _dbUrl + "login";

            var response = await _httpClient.PostAsync(loginUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

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

        public async Task<Users> GetUserById(Guid id)
        {
            var response = await _httpClient.GetAsync(_dbUrl + "getUser/" + id);

            if(response == null)
            {
                return null;
            }

            // Read and deserialize the response content
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Users>(jsonResponse);

            return user;
        }
    }
    
}
