using Blazored.LocalStorage;
using DrawCurve.Client.Provider.DrawCurve.Client.Provider;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using DrawCurve.Domen.Responces;
using DrawCurve.Domen.Models;

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

        public async Task<(bool Success, string Message)> RegisterAsync(ResponceRegistration request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/Registration", request);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Registration successful");
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            return (false, errorResponse.Message);
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            return token != null;
        }
    }
}
