using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private IRenderService renderService;
        private IVideoService videoService;

        public VideoController(IRenderService renderService, IVideoService videoService)
        { 
            this.renderService = renderService;
            this.videoService = videoService;
        }
        [HttpGet("Page/{i?}")]
        public IEnumerable<VideoInfo> GetVideoInfo(int? i, bool Shafle)
        {
            int pageNumber = i ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            return videoService.GetVideoInfos(pageNumber, Shafle);
        }
        [HttpGet("{renderId}")]
        public IActionResult Get(string renderId)
        {
            var path = videoService.GetVideo(renderId);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var result = new FileStreamResult(stream, "video/mp4") // Укажите нужный MIME-тип
            {
                EnableRangeProcessing = true // Позволяет перематывать
            };

            return result;
        }


        [HttpGet("{renderId}/Info")]
        public VideoInfo Info(string renderId)
        {
            return videoService.GetInfo(renderId);
        }

        [HttpGet("{RenderKey}/Frame")]
        public ActionResult<FileStream> GetFrame(string RenderKey)
        {
            try
            {
                var render = renderService.GetRender(RenderKey);

                var path = DirectoryHelper.GetPathToSaveFrame(RenderKey);
                var files = Directory.GetFiles(path);

                // Проверяем, что файлов больше 100
                if (files.Length < 100 && files.Length != 1)
                {
                    return NotFound(); // Или любое другое подходящее сообщение
                }

                var paths = files.OrderBy(x => x).ToList();
                var pathToImage = render?.Status < Domen.Models.Menedger.TypeStatus.ProccessConcatFrame ? paths[paths.Count - 20] : paths.First();

                // Открываем файл для чтения
                var stream = new FileStream(pathToImage, FileMode.Open, FileAccess.Read);
                return File(stream, "image/png"); // Замените "image/jpeg" на нужный вам MIME-тип
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("Publish")]
        public void Publish(VideoResponce video)
        {
            var user = ControllerContext.HttpContext.User;
            videoService.Publish(video, int.Parse(user.FindFirst("Id").Value));
        }
    }
}
