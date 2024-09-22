using DrawCurve.Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly string _storagePath = DirectoryHelper.PathToAudio();

        public MusicController()
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        // Метод для загрузки и сохранения файла
        [Authorize]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не выбран или пуст.");
            }

            // Создаем уникальное имя файла, чтобы избежать конфликтов
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_storagePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/api/music/get/{fileName}";
            return Ok(fileUrl);
        }

        // Метод для получения файла по его имени
        [HttpGet("get/{fileName}")]
        public IActionResult Get(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Файл не найден.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "*/*", fileName);
        }
    }
}
