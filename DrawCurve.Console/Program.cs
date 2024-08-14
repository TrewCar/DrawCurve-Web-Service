using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Window;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFML.Graphics;
using SFML.Window;
using System.Reflection;
using DrawCurve.Domen.DTO.Models;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var renderConfig2 = new CurveRender();
        var renderConfig = renderConfig2.GetDefaultRenderConfig() ;



        renderConfig.Colors["background"] = new Color(0, 0, 0, 3);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Follow);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Zoom);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Speed);

        renderConfig.FPS = 5000;

        renderConfig.DeltaTime = TypeDeltaTime.Fixed;

        var obj = new List<ObjectRender>
        {
            new LineCurve(100, 90, 2*MathF.PI),
            new LineCurve(100, 90, -MathF.PI/15),
        };

        MenedgerSaveRender render = new MenedgerSaveRender(renderConfig, obj);
        render.Init();
        render.Start();

    }
}

public class MenedgerSaveRender : CurveRender
{
    public MenedgerSaveRender(RenderConfig config, List<ObjectRender> Objects) : base(config, Objects)
    {

    }
    public override bool TickRender(float deltaTime)
    {
        var res = base.TickRender(deltaTime);


#if DEBUG

#else
        if (RenderConfig.Time * RenderConfig.FPS < CountFrame)
            return false;
        window.Texture.CopyToImage().SaveToFile(Path.Combine("TEST", $"{new Guid().ToString()}frame_{CountFrame:D5}.png"));
#endif


        return res;
    }
}