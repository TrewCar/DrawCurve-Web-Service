using DrawCurve.Application.Menedgers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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
        [Route("List")]
        public IEnumerable<string> GetInfoRenders()
        {
            return config.GetInfoRenders();
        }

        [HttpGet]
        [Route("Template")]
        public Domen.Models.Core.RenderConfig GetTemplate()
        {
            return new Domen.Models.Core.RenderConfig();
        }

        [HttpGet]
        [Route("{RenderName}/Default")]
        public Domen.Models.Core.RenderConfig GetConfig(string RenderName)
        {
            return config.GetDefaultConfig(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Objects")]
        public IEnumerable<Domen.Models.Core.Objects.ObjectRender> GetConfigObjects(string RenderName)
        {
            return config.GetConfigObjects(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Colors")]
        public Dictionary<string, Domen.Models.Core.Color> GetConfigColor(string RenderName)
        {
            return config.GetConfigColor(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Actions")]
        public IEnumerable<Domen.Models.Core.ActionConfig> GetConfigActions(string RenderName)
        {
            return config.GetConfigActions(RenderName);
        }

        [HttpGet]
        [Route("{RenderName}/Tags")]
        public IEnumerable<Domen.Models.Core.Enums.TagRender> GetConfigTags(string RenderName)
        {
            return config.GetConfigTags(RenderName);
        }
    }
}
