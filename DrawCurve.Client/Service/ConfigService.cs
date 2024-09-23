using Blazored.LocalStorage;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;
using DrawCurve.Domen.Responces;
using System.Net.Http.Json;

namespace DrawCurve.Client.Service
{
    public class ConfigService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ConfigService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<RenderConfig> GetDefaultData(RenderType type)
        {
            var response = await _httpClient.GetAsync($"api/Config/{type}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception();
            }

            return await response.Content.ReadFromJsonAsync<RenderConfig>();
        }
        public async Task<List<ObjectRender>> GetDefaultObjects(RenderType type)
        {
            var response = await _httpClient.GetAsync($"api/Config/{type}/Objects");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception();
            }

            return await response.Content.ReadFromJsonAsync<List<ObjectRender>>();
        }
    }
}
