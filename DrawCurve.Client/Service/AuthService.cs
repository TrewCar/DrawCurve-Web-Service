using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DrawCurve.Client.Service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
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

        public async Task<LoginResponse> LoginAsync(UserResurce loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/Login", loginModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Token != null)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/Registration", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
    }
    public class RegistrationRequest
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserResurce
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class RegistrationResponse
    {
        public string Message { get; set; }
    }

}
