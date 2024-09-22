using DrawCurve.Core.Window;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core.Objects;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerConfig
    {
        public MenedgerConfig() { }

        public Domen.Models.Core.RenderConfig GetDefaultConfig(RenderType nameRender)
        {
            switch (nameRender)
            {
                case RenderType.RenderCurve:
                    Render render1 = new CurveRender();
                    var item1 = render1.GetDefaultRenderConfig().Transfer();
                    return item1;
                case RenderType.LissajousFigures:
                    Render render2 = new LisajuFormsRender();
                    var item2 = render2.GetDefaultRenderConfig().Transfer();
                    return item2;
                default:
                    throw new NotImplementedException($"Type {nameRender} is not implemented in {GetType().FullName}");
            }

        }

        public Dictionary<string, Domen.Models.Core.Color> GetConfigColor(RenderType nameRender)
        {
            return GetDefaultConfig(nameRender).Colors;
        }

        public IEnumerable<Domen.Models.Core.ActionConfig> GetConfigActions(RenderType nameRender)
        {
            return GetDefaultConfig(nameRender).ActionsConfig;
        }

        public IEnumerable<Domen.Models.Core.Enums.TagRender> GetConfigTags(RenderType nameRender)
        {
            return GetDefaultConfig(nameRender).Tags;
        }

        public IEnumerable<Domen.Models.Core.Objects.ObjectRender> GetConfigObjects(RenderType nameRender)
        {
            if (nameRender == RenderType.RenderCurve)
            {
                return new List<Domen.Models.Core.Objects.ObjectRender>
                {
                    new LineCurve(),
                };
            }
            else if (nameRender == RenderType.LissajousFigures)
            {
                return new List<Domen.Models.Core.Objects.ObjectRender>();
            }
            return new List<ObjectRender>();
        }
    }
}
