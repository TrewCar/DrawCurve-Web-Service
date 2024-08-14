using DrawCurve.Core.Config;
using DrawCurve.Core.Menedger;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Window;
using DrawCurve.Domen.DTO.Models.Objects;
using SFML.Graphics;
using System.Text.Json;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var renderConfig2 = new CurveRender();
        var renderConfig = renderConfig2.GetDefaultRenderConfig();



        renderConfig.Colors["background"] = new Color(0, 0, 0, 3);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Follow);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Zoom);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Speed);

        renderConfig.FPS = 5000;
        renderConfig.Time = 10;

        renderConfig.DeltaTime = TypeDeltaTime.Fixed;

        renderConfig.Width = 1000;
        renderConfig.Height = 1000;

        var obj = new List<ObjectRender>
        {
            new LineCurve(100, 90, 2*MathF.PI),
            new LineCurve(100, 90, -MathF.PI/15),
        };

        var objModel = obj.Select(x => x.Transfer()).ToList();

        // Сериализация:
        string json = JsonSerializer.Serialize(objModel, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);

        // Десериализация:
        var deserializedObjects = JsonSerializer.Deserialize<List<DrawCurve.Domen.Models.Core.Objects.ObjectRender>>(json);

        CurveRender render = new CurveRender(renderConfig, deserializedObjects.Select(x => x.Transfer()).ToList());

        MenedgerRender menedger = new(render);

        menedger.OnSendStatusRender += (status) =>
        {
            Console.WriteLine($"FPS: {status.FPS} \t CountFrame: {status.CountFPS} \t MaxFrameCount: {status.MaxCountFPS}");
        };

        menedger.OnEndProccess += () =>
        {
            Console.WriteLine("END PROCCESS");
        };

        menedger.Init();
        menedger.Start();
    }
}