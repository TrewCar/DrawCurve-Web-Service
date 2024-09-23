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
            var response = await _httpClient.GetAsync("api/Render/all");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new List<RenderInfo>(); // Возвращаем пустой список в случае ошибки
            }

            return await response.Content.ReadFromJsonAsync<List<RenderInfo>>() ?? new List<RenderInfo>();
        }

        public async Task<RenderInfo> GetRenderCur(string Key)
        {
            var response = await _httpClient.GetAsync($"api/Render/{Key}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new RenderInfo(); // Возвращаем новый пустой объект в случае ошибки
            }

            return await response.Content.ReadFromJsonAsync<RenderInfo>() ?? new RenderInfo();
        }

        public async Task<string> StartRender(RenderType renderType, ResponceRenderInfo render)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Render/{renderType}/Generate", render);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return ""; // Обрабатываем ошибку
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
