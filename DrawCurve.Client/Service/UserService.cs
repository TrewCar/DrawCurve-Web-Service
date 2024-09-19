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

        private async Task AddAuthorizationHeaderAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<User> GetUserInfoAsync()
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetFromJsonAsync<User>("api/user/info");
        }
    }
}
