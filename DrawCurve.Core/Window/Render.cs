using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using DrawCurve.Core.Objects;
using SFML.Graphics;

namespace DrawCurve.Core.Window
{
    public abstract class Render: IDisposable
    {
        public string KEY { get; set; }

        public RenderWindow window { get; set; }

        public List<ObjectRender> Objects = new();

        protected delegate void UpdateAction(float deltaTime);
        protected UpdateAction TickAction;

        public delegate void DoneFrame(string key);
        public DoneFrame OnDoneFrame;

        public delegate void CommpliteRender(string key);
        public CommpliteRender OnCompliteRender;


        public int CountFrame { get; set; } = 0;
        public float SpeedRender = 1f;

        public RenderConfig RenderConfig { get; private set; }

        public FPSCounter fpsCounter = new FPSCounter();

        public Color background = Color.Black;

        public bool Close = false;

        private int frameSmooth;


        public virtual List<ActionBase> active { get; set; } = new List<ActionBase>();

        public Render()
        {
            this.RenderConfig = GetDefaultRenderConfig();
            this.Objects = new List<ObjectRender>();
        }


        public Render(RenderConfig config, List<ObjectRender> Objects)
        {
            this.RenderConfig = config;
            this.Objects = Objects;
        }

        public virtual void Init()
        {
            this.SpeedRender = RenderConfig.SpeedRender;

            this.window = new RenderWindow(new SFML.Window.VideoMode(RenderConfig.Width, RenderConfig.Height), KEY);

            this.active.ForEach(X => X.SetConfig(RenderConfig));

            this.frameSmooth = RenderConfig.IndexSmooth;

            this.window.SetVisible(true);
        }

        public void Start()
        {
            while (this.RenderConfig.FPS * this.RenderConfig.Time >= this.CountFrame && !Close)
            {
                fpsCounter.Update();
                
                float deltaTime = (1.0F / RenderConfig.FPS * SpeedRender) / RenderConfig.IndexSmooth;

                this.TickAction?.Invoke(deltaTime);

                var val = TickRender(deltaTime);
                window.Display();

                if (frameSmooth == RenderConfig.IndexSmooth)
                {
                    frameSmooth = 0;
                    CountFrame++;
                    OnDoneFrame?.Invoke(KEY);
                }

                frameSmooth++;

            }
            window.Close();
            OnCompliteRender?.Invoke(KEY);
        }

        /// <summary>
        /// Рисуйте сударь
        /// </summary>
        /// <returns></returns>
        public abstract bool TickRender(float deltaTime);

        public abstract RenderConfig GetDefaultRenderConfig();

        public void Dispose()
        {
            window.Dispose();
        }
    }
}
