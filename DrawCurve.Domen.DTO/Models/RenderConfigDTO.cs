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
                Title = cnf.Title,
                Tags = cnf.Tags.Select(x=> 
                    x.DataTransfer<TagRenderModel, TagRenderCore>()
                ).ToList(),

                FPS = cnf.FPS,
                Time = 20,
                SpeedRender = 1,

                IndexSmooth = cnf.IndexSmooth,

                Width = 1080,
                Height = 1920,

                ActionsConfig = cnf.ActionsConfig.Select(x => x.Transfer()).ToList(),

                Colors = cnf.Colors.Select(x =>
                    new KeyValuePair<string, ColorCore>(x.Key, x.Value.Transfer())
                ).ToDictionary(),
            };

        public static RenderConfigModel Transfer(this RenderConfigCore cnf)
            => new RenderConfigModel()
            {
                Title = cnf.Title,
                Tags = cnf.Tags.Select(x =>
                    x.DataTransfer<TagRenderCore, TagRenderModel>()
                ).ToList(),

                FPS = cnf.FPS,
                Time = 20,
                SpeedRender = 1,

                IndexSmooth = cnf.IndexSmooth,

                Width = 1080,
                Height = 1920,

                ActionsConfig = cnf.ActionsConfig.Select(x => x.Transfer()).ToList(),

                Colors = cnf.Colors.Select(x =>
                    new KeyValuePair<string, ColorModel>(x.Key, x.Value.Transfer())
                ).ToDictionary(),
            };

    }
}
