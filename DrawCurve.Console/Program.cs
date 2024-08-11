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
            new LineCurve(100, 90, -MathF.PI/15),
        });

        var renderConfig = render.GetDefaultRenderConfig();

        new Thread(() => { 
        render.Start();
        }).Start();
    }
}