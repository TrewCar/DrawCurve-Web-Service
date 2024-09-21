using DrawCurve.Application.Menedgers;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    /// <summary>
    /// Контроллер для получения стандартных конфигурационных файлов для рендера видео
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private MenedgerConfig config;
        public ConfigController(MenedgerConfig config)
        {
            this.config = config;
        }


        [HttpGet]
        [Route("Template")]
        public Domen.Models.Core.RenderConfig GetTemplate()
        {
            return new Domen.Models.Core.RenderConfig();
        }

        [HttpGet]
        [Route("{RenderName}/Default")]
        public Domen.Models.Core.RenderConfig GetConfig(RenderType RenderName)
        {
            return config.GetDefaultConfig(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Objects")]
        public IEnumerable<Domen.Models.Core.Objects.ObjectRender> GetConfigObjects(RenderType RenderName)
        {
            return config.GetConfigObjects(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Colors")]
        public Dictionary<string, Domen.Models.Core.Color> GetConfigColor(RenderType RenderName)
        {
            return config.GetConfigColor(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Actions")]
        public IEnumerable<Domen.Models.Core.ActionConfig> GetConfigActions(RenderType RenderName)
        {
            return config.GetConfigActions(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Tags")]
        public IEnumerable<Domen.Models.Core.Enums.TagRender> GetConfigTags(RenderType RenderName)
        {
            return config.GetConfigTags(RenderName);
        }
    }
}
