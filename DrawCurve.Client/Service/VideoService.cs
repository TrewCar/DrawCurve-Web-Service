using Blazored.LocalStorage;

namespace DrawCurve.Client.Service
{
    public class VideoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public VideoService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<Stream> GetRenderImage(string key)
        {
            var response = await _httpClient.GetAsync($"api/Video/{key}/Frame");

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
    }
}
