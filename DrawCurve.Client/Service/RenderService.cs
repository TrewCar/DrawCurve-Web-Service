using Blazored.LocalStorage;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DrawCurve.Client.Service
{
    public class RenderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public RenderService(HttpClient httpClient, ILocalStorageService localStorage)
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

        public async Task<List<RenderInfo>> GetRenderList()
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync("api/Render/get/all/self");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new List<RenderInfo>(); // Возвращаем пустой список в случае ошибки
            }

            return await response.Content.ReadFromJsonAsync<List<RenderInfo>>() ?? new List<RenderInfo>();
        }

        public async Task<RenderInfo> GetRenderCur(string Key)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Render/get/{Key}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new RenderInfo(); // Возвращаем новый пустой объект в случае ошибки
            }

            return await response.Content.ReadFromJsonAsync<RenderInfo>() ?? new RenderInfo();
        }

        public async Task<string> StartRender(RenderType renderType, ResponceRenderInfo render)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync($"api/Render/generate/{renderType}", render);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null; // Обрабатываем ошибку
            }

            return await response.Content.ReadAsStringAsync();
        }

        public class ResponceRenderInfo
        {
            public string Name { get; set; }
            public List<ObjectRender> obejcts { get; set; }
            public RenderConfig config { get; set; }
        }

        public async Task<RenderConfig> GetDefaultData(RenderType type)
        {

        }
    }
}
