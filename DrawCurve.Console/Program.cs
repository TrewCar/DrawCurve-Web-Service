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

        renderConfig.FPS = 5000;

        renderConfig.DeltaTime = TypeDeltaTime.Fixed;

        renderConfig.Objects = new List<ObjectRender>
        {
            new LineCurve(100, 90, 2*MathF.PI),
            new LineCurve(100, 90, -MathF.PI/15),
        };


        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
        serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
        serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
        serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

        using (StreamWriter sw = new StreamWriter("test.json"))
        using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
        {
            serializer.Serialize(writer, renderConfig, typeof(RenderConfig));
        }

        RenderConfig cnf = Newtonsoft.Json.JsonConvert.DeserializeObject<RenderConfig>(File.ReadAllText("test.json"), new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
        });


        CurveRender render = new CurveRender(cnf);
        render.Start();
    }
}