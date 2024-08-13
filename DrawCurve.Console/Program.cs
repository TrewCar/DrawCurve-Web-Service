using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Window;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFML.Graphics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var renderConfig2 = new CurveRender();
        var renderConfig = renderConfig2.GetDefaultRenderConfig() ;
        renderConfig2.Dispose();



        renderConfig.Colors["background"] = new Color(0, 0, 0, 3);
        renderConfig.Tags.Remove(DrawCurve.Tags.TagRender.Follow);
        renderConfig.Tags.Remove(DrawCurve.Tags.TagRender.Zoom);
        renderConfig.Tags.Remove(DrawCurve.Tags.TagRender.Speed);

        renderConfig.FPS = 1000;

        renderConfig.DeltaTime = TypeDeltaTime.Fixed;



        CurveRender render = new CurveRender(renderConfig, new List<LineCurve>
        {
            new LineCurve(100, 90, 2*MathF.PI),
            new LineCurve(100, 90, -MathF.PI/15),
        });
        render.Start();
    }
}