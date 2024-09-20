namespace DrawCurve.Client.Provider
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Threading.Tasks;

    namespace DrawCurve.Client.Provider
    {
        public class CustomAuthenticationStateProvider : AuthenticationStateProvider
        {
            private readonly ILocalStorageService _localStorage;
            private readonly HttpClient _httpClient;

            public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
            {
                _localStorage = localStorage;
                _httpClient = httpClient;
            }

            public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // Установим заголовок Authorization для дальнейших запросов
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwtAuthType");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }

            public async Task MarkUserAsAuthenticated(string token)
            {
                await _localStorage.SetItemAsync("authToken", token);

                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwtAuthType");
                var user = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            }

            public async Task MarkUserAsLoggedOut()
            {
                await _localStorage.RemoveItemAsync("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = null;

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
            }

            private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);

                return token.Claims;
            }
        }
    }

}
