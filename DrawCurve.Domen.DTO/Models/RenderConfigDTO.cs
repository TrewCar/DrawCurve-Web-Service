using ColorCore = SFML.Graphics.Color;
using ColorModel = DrawCurve.Domen.Models.Core.Color;
using RenderConfigCore = DrawCurve.Core.Config.RenderConfig;
using RenderConfigModel = DrawCurve.Domen.Models.Core.RenderConfig;
using TagRenderCore = DrawCurve.Core.Tags.TagRender;
using TagRenderModel = DrawCurve.Domen.Models.Core.Enums.TagRender;

namespace DrawCurve.Domen.DTO.Models
{
    public static class RenderConfigDTO
    {
        public static RenderConfigCore Transfer(this RenderConfigModel cnf)
            => new RenderConfigCore()
            {
                Tags = cnf.Tags.Select(x =>
                    x.DataTransfer<TagRenderModel, TagRenderCore>()
                ).ToList(),

                FPS = (uint)cnf.FPS,
                Time = cnf.Time,
                SpeedRender = 1,

                PathMusic = cnf.PathMusic,

                IndexSmooth = cnf.IndexSmooth,

                Width = (uint)cnf.Width,
                Height = (uint)cnf.Height,

                ActionsConfig = cnf.ActionsConfig.Select(x => x.Transfer()).ToList(),

                Colors = cnf.Colors.Select(x =>
                    new KeyValuePair<string, ColorCore>(x.Key, x.Value.Transfer())
                ).ToDictionary(),
            };

        public static RenderConfigModel Transfer(this RenderConfigCore cnf)
            => new RenderConfigModel()
            {
                Tags = cnf.Tags.Select(x =>
                    x.DataTransfer<TagRenderCore, TagRenderModel>()
                ).ToList(),

                FPS = (int)cnf.FPS,
                Time = cnf.Time,
                SpeedRender = 1,

                PathMusic = cnf.PathMusic,

                IndexSmooth = cnf.IndexSmooth,

                Width = (int)cnf.Width,
                Height = (int)cnf.Height,

                ActionsConfig = cnf.ActionsConfig.Select(x => x.Transfer()).ToList(),

                Colors = cnf.Colors.Select(x =>
                    new KeyValuePair<string, ColorModel>(x.Key, x.Value.Transfer())
                ).ToDictionary(),
            };

    }
}
