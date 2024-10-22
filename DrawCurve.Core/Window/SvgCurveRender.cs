using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Tags;
using SFML.Graphics;
using SvgPathProperties;
using System.Numerics;
using System.Xml;

namespace DrawCurve.Core.Window
{
    public class SvgCurveRender : CurveRender
    {
        private static int Components = 1000;
        private static double Step = 0.001;
        public SvgCurveRender() { }
        public SvgCurveRender(RenderConfig config, List<ObjectRender> Objects) : base(config, Objects)
        {
            var SVg = (SvgCurve)Objects.Find(x => x is SvgCurve);
            if (SVg == null)
                throw new Exception("В списке обьектов должен содержаться обьект SvgCurve");

            SpeedRender = 0.03f;

            this.Objects = CalcLineCurve(SVg);
        }

        public override List<ActionBase> active { get; set; } = new()
        {
            new ZoomAction(),
            new FollowAction(),
            new SpeedAction(),
        };
        private List<ObjectRender> CalcLineCurve(SvgCurve SVG)
        {
            var path = new List<Complex>();
            //var svg = SvgToPathConverter.SvgToPath(pathToSVG);

            XmlDocument svgDocument = new XmlDocument();
            svgDocument.LoadXml(SVG.SvgValue);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(svgDocument.NameTable);
            nsmgr.AddNamespace("svg", "http://www.w3.org/2000/svg");

            XmlNodeList pathNodes = svgDocument.SelectNodes("//svg:path", nsmgr);

            foreach (XmlNode pathNode in pathNodes)
            {
                string dAttribute = pathNode.Attributes["d"]?.Value;

                if (!string.IsNullOrEmpty(dAttribute))
                {
                    var temp = new SvgPath(dAttribute);
                    var lenght = temp.Length;
                    //var path1 = temp.GetParts();


                    for (int i = 0; i < lenght; i++)
                    {
                        var point = temp.GetPointAtLength(i);
                        path.Add(new Complex(point.X, point.Y));
                    }

                }

            }
            double real = path.Sum(x => x.Real) / path.Count;
            double imaginary = path.Sum(x => x.Imaginary) / path.Count;

            // Размер холста
            double canvasWidth = this.RenderConfig.Width * 0.8;
            double canvasHeight = this.RenderConfig.Height * 0.8;

            // Найти минимальные и максимальные значения координат X и Y
            double minX = path.Min(p => p.Real);
            double maxX = path.Max(p => p.Real);
            double minY = path.Min(p => p.Imaginary);
            double maxY = path.Max(p => p.Imaginary);

            // Вычислить ширину и высоту, занимаемую точками
            double pointsWidth = maxX - minX;
            double pointsHeight = maxY - minY;

            // Вычислить масштабный коэффициент
            double scaleReal = canvasWidth / pointsWidth;
            double scaleImaginary = canvasHeight / pointsHeight;
            double scale = Math.Min(scaleReal, scaleImaginary);

            int reverse = 1;

            path = path.Select(x =>
            {
                return new Complex(
                   ((x.Real * scale) - real * scale) * reverse,
                   ((x.Imaginary * scale) - imaginary * scale) * reverse);
            }
            ).ToList();

            return FourierTransform.CalculateArrows(path, Components, Step);
        }
        public override RenderConfig GetDefaultRenderConfig()
        {
            var ActionsConfig = active.Select(x => x.GetDefaultConfig()).ToList();

            return new RenderConfig()
            {
                Title = "Title name",
                Tags = new List<TagRender>()
                {
                    TagRender.RenderHand,
                    TagRender.RenderCurve,
                    TagRender.Follow,
                    TagRender.Unfollow,
                    TagRender.Speed,
                    TagRender.Zoom
                },
                FPS = 144,
                Time = 60,

                IndexSmooth = 1,

                SpeedRender = 0.03f,
                Width = 1080,
                Height = 1920,

                ActionsConfig = new List<ActionConfig>
                { 
                    new ActionConfig()
                    {
                        Key = "FollowAction",
                        Name = "Слежение за обьектом",
                        Description = "Центрирование массива точек относительно N точки",

                        Step = 0.3f,

                        Start = 0.1f,
                        End = 0.7f
                    },
                    new ActionConfig()
                    {
                        Key = "SpeedAction",
                        Name = "Замедление",
                        Description = "Замедление рендера",

                        Step = 0.001f,

                        Start = 0.2f,
                        End = 0.6f,

                        MaxValue = 0.03f,
                        MinValue = 0.01f,
                    },
                    new ActionConfig()
                    {
                        Key = "ZoomAction",
                        Name = "Приближение",
                        Description = "Приближение к N точке",

                        Step = 0.3f,

                        Start = 0.2f,
                        End = 0.55f,

                        MaxValue = 15.0f,
                        MinValue = 1.0f,
                    }
                },

                Colors = new()
                {
                    { "background", Color.Black },
                    { "stylus", Color.Red },
                    { "curve", Color.White },
                },
            };
        }
    }
}
