using DrawCurve.Core.Window;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.Models.Core.Objects;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerConfig
    {
        public MenedgerConfig() { }

        public List<string> GetInfoRenders()
        {
            return new List<string>
            {
                "CurveRender",
            };
        }

        public Domen.Models.Core.RenderConfig GetDefaultConfig(string nameRender)
        {
            Render render = new CurveRender();
            return render.GetDefaultRenderConfig().Transfer();
        }

        public Dictionary<string, Domen.Models.Core.Color> GetConfigColor(string nameRender)
        {
            return GetDefaultConfig(nameRender).Colors;
        }

        public IEnumerable<Domen.Models.Core.ActionConfig> GetConfigActions(string nameRender)
        {
            return GetDefaultConfig(nameRender).ActionsConfig;
        }

        public IEnumerable<Domen.Models.Core.Enums.TagRender> GetConfigTags(string nameRender)
        {
            return GetDefaultConfig(nameRender).Tags;
        }

        public IEnumerable<Domen.Models.Core.Objects.ObjectRender> GetConfigObjects(string nameRender)
        {
            if (nameRender == "CurveRender")
            {
                return new List<Domen.Models.Core.Objects.ObjectRender>
                {
                    new LineCurve(),
                };
            }
            return new List<ObjectRender>();
        }
    }
}
