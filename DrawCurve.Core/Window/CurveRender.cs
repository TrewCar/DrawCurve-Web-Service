using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Tags;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Window
{
    public class CurveRender : Render
    {
        protected List<Vertex> curve = new List<Vertex>();
        protected List<LineCurve> _lines;

        public override List<ActionBase> active { get; set; } = new()
        {
            new ZoomAction(),
            new FollowAction(),
            new SpeedAction(),
        };

        public CurveRender(List<LineCurve> stylus) : base()
        {
            this._lines = stylus;
            InitUpdateSylus();
        }

        public CurveRender(RenderConfig config, List<LineCurve> stylus) : base(config)
        {
            this._lines = stylus;
            InitUpdateSylus();
        }

        private void InitUpdateSylus()
        {
            foreach (var item in _lines)
            {
                this.TickAction += item.Update;
            }
        }
        

        public override bool TickRender(float deltaTime)
        {
            window.Clear(RenderConfig.Colors["background"]);

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
            Vertex[] array = new Vertex[_lines.Count + 1];
            array[0] = new Vertex(pos, RenderConfig.Colors["stylus"]);

            Vector2f vec = pos;

            for (int i = 0; i < _lines.Count; i++)
            {
                vec = _lines[i].GetPoint(vec);
                array[i + 1] = new Vertex(vec, RenderConfig.Colors["stylus"]);
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

        public override RenderConfig GetRenderConfig()
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
                    TagRender.Million
                },
                FPS = 144,
                Time = 20,
                SpeedRender = 1,
                DeltaTime = TypeDeltaTime.Fixed,
                Width = 1080,
                Height = 1920,

                ActionsConfig = active.Select(x => x.GetDefaultConfig()).ToList(),

                Colors = new()
                {
                    {"background", Color.Black },
                    {"stylus", Color.Red},
                    {"curve", Color.White},
                },
            };
        }
    }
}
