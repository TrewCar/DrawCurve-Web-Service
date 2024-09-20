using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using DrawCurve.Client.Provider.DrawCurve.Client.Provider;

namespace DrawCurve.Client.Service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<LoginResponse> LoginAsync(UserResource loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/Login", loginModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Token != null)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                // Обновляем состояние аутентификации
                await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            // Обновляем состояние аутентификации
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/Registration", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            return token != null;
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

    public class UserResource
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RegistrationResponse
    {
        public string Message { get; set; }
    }
}
