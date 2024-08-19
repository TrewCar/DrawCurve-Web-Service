using DrawCurve.Application.Menedgers;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Window;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.DTO.Models.Objects;
using SFML.Graphics;
using System.Text.Json;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        for (int i = 0; i < 1; i++)
        {
            InitCreate();
        }
    }
    private static void InitCreate()
    {
        var renderConfig2 = new CurveRender();
        var renderConfig = renderConfig2.GetDefaultRenderConfig();

        renderConfig.Colors["background"] = new Color(0, 0, 0, 3);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Follow);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Zoom);
        renderConfig.Tags.Remove(DrawCurve.Core.Tags.TagRender.Speed);

        renderConfig.FPS = 200;
        renderConfig.Time = 100;

        renderConfig.IndexSmooth = 50;

        renderConfig.Width = 1000;
        renderConfig.Height = 1000;

        var obj = new List<ObjectRender>
        {
            new LineCurve(100, 90, 2*MathF.PI),
            new LineCurve(100, 90, -MathF.PI/15),
        };

        var objModel = obj.Select(x => x.Transfer()).ToList();

        //// Сериализация:
        string json = JsonSerializer.Serialize(objModel, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);

        Console.WriteLine();

        json = JsonSerializer.Serialize(renderConfig.Transfer(), new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);

        //// Десериализация:
        //var deserializedObjects = JsonSerializer.Deserialize<List<DrawCurve.Domen.Models.Core.Objects.ObjectRender>>(json);

        CurveRender render = new CurveRender(renderConfig, obj);


        //MenedgerRender menedger = new();
        //menedger.Add(1, render);
    }
}