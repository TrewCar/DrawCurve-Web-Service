using Blazored.LocalStorage;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;
using DrawCurve.Domen.Responces;
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

        public async Task<List<RenderInfo>> GetRenderList()
        {
            var response = await _httpClient.GetAsync("api/Render/get/all/self");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new List<RenderInfo>(); // Возвращаем пустой список в случае ошибки
            }

            return await response.Content.ReadFromJsonAsync<List<RenderInfo>>() ?? new List<RenderInfo>();
        }

        public async Task<RenderInfo> GetRenderCur(string Key)
        {
            var response = await _httpClient.GetAsync($"api/Render/get/{Key}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new RenderInfo(); // Возвращаем новый пустой объект в случае ошибки
            }

            return await response.Content.ReadFromJsonAsync<RenderInfo>() ?? new RenderInfo();
        }

        public async Task<Stream> GetRenderImage(string key)
        {
            var response = await _httpClient.GetAsync($"api/Render/get/{key}/frame");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Возвращаем изображение по умолчанию
                return null;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Unauthorized access.");
            }
            else if (response.IsSuccessStatusCode)
            {
                // Возвращаем FileStream
                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                // Возвращаем null или выбрасываем исключение для других статусов
                throw new Exception("Error fetching render image.");
            }
        }


        public async Task<string> StartRender(RenderType renderType, ResponceRenderInfo render)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Render/generate/{renderType}", render);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return ""; // Обрабатываем ошибку
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<RenderConfig> GetDefaultData(RenderType type)
        {
            var response = await _httpClient.GetAsync($"api/Config/{type}/Default");

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
