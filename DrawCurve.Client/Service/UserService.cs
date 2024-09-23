using Blazored.LocalStorage;
using DrawCurve.Domen.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DrawCurve.Client.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<User> GetUserInfoAsync()
        {
            return await _httpClient.GetFromJsonAsync<User>("api/user");
        }
    }
}
