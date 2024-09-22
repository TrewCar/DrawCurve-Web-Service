using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace DrawCurve.Client.Service
{
    public class MusicService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public MusicService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<string> Save(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 104857600)); // Ограничение в 100 МБ
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            var response = await _httpClient.PostAsync("api/music/save", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result; // Возвращаем URL загруженного файла
        }

        public async Task<Stream> Get(string fileName)
        {
            var response = await _httpClient.GetAsync($"api/music/get/{fileName}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }
    }

}
