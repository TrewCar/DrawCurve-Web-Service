using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Tags;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Window
{
    public class CurveRender : Render
    {
        protected List<Vertex> curve = new();


        public override List<ActionBase> active { get; set; } = new()
        {
            new ZoomAction(),
            new FollowAction(),
            new SpeedAction(),
        };

        public CurveRender() : base() { }

        public CurveRender(RenderConfig config, List<ObjectRender> Objects) : base(config, Objects)
        {
        }

        public override void Init()
        {
            base.Init();
            foreach (var item in objects)
            {
                if (item is LineCurve line)
                    this.TickAction += item.Update;
            }
        }
        

        public override bool TickRender(float deltaTime)
        {
            var background = new RectangleShape(new Vector2f(window.Size.X, window.Size.Y));
            background.FillColor = RenderConfig.Colors["background"];
            window.Draw(background);

            var vecs = CalcLines(new Vector2f(window.Size.X / 2, window.Size.Y / 2));

            curve.Add(new Vertex(vecs[vecs.Length - 1].Position, RenderConfig.Colors["curve"]));

            var center = vecs.Last().Position;

            vecs = Actions(center, vecs, deltaTime);
            var points = Actions(center, curve.ToArray(), deltaTime);



            if (RenderConfig.Tags.Contains(TagRender.RenderCurve))
                window.Draw(points, PrimitiveType.LineStrip);

            if (RenderConfig.Tags.Contains(TagRender.RenderHand))
                window.Draw(vecs, PrimitiveType.LineStrip);

            return true;
        }

        private Vertex[] CalcLines(Vector2f pos)
        {
            Vertex[] array = new Vertex[objects.Count + 1];
            array[0] = new Vertex(pos, RenderConfig.Colors["stylus"]);

            Vector2f vec = pos;

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is LineCurve line)
                {
                    vec = line.GetPoint(vec);
                    array[i + 1] = new Vertex(vec, RenderConfig.Colors["stylus"]);
                }
            }
            return array;
        }

        private Vertex[] Actions(Vector2f pos, Vertex[] array, float deltaTime)
        {
            foreach (var action in active)
            {
                array = action.Action(pos, array, deltaTime, this);
            }
            return array;
        }

        public override RenderConfig GetDefaultRenderConfig()
        {
            var ActionsConfig = active.Select(x=>x.GetDefaultConfig()).ToList();

            return new RenderConfig()
            {
                Title = "Title name",
                Tags = new List<TagRender>()
                {
                    TagRender.RenderHand,
                    TagRender.RenderCurve,
                    TagRender.Follow,
                    TagRender.Unfollow,
                    TagRender.Zoom,
                    TagRender.Speed,
                },
                FPS = 144,
                Time = 20,

                IndexSmooth = 1,
                 
                SpeedRender = 1,
                Width = 1080,
                Height = 1920,

                ActionsConfig = active.Select(x => x.GetDefaultConfig()).ToList(),

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
