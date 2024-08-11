using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Window;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");


        CurveRender render = new CurveRender(new List<LineCurve>
        {
            new LineCurve(100, 90, 2*MathF.PI),
            new LineCurve(100, 90, -MathF.PI/3),
        });
        var renderConfig = render.GetRenderConfig();

        new Thread(() => { 
        render.Start();
        }).Start();

        // Convert RenderConfig object to JSON
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(), // Handles enum as string
            },
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(renderConfig, settings);
        Console.WriteLine("Serialized JSON:\n" + json);

        // Deserialize JSON back to RenderConfig object
        var deserializedConfig = JsonConvert.DeserializeObject<RenderConfig>(json, settings);
        Console.WriteLine("\nDeserialized RenderConfig: " + JsonConvert.SerializeObject(deserializedConfig, settings));
    }
}