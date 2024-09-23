using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
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

        public VideoController(IRenderService renderService)
        { 
            this.renderService = renderService;
        }
        [HttpGet("{renderId}")]
        public void Get(string renderId)
        {

        }

        [HttpGet("{renderId}/Info")]
        public void Info(string renderId)
        {

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
                if (files.Length < 100)
                {
                    return NotFound(); // Или любое другое подходящее сообщение
                }

                var paths = files.OrderBy(x => x).ToList();
                var pathToImage = render?.Status != Domen.Models.Menedger.TypeStatus.ProccessEnd ? paths[paths.Count - 20] : paths[(int)(paths.Count / 2)];

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
        [HttpPost("{renderId}/Publish")]
        public void Publish(string renderId)
        {
            var user = ControllerContext.HttpContext.User;
        }
    }
}
