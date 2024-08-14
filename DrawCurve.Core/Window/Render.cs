using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using DrawCurve.Core.Objects;
using SFML.Graphics;
using SFML.Window;

namespace DrawCurve.Core.Window
{
    public abstract class Render : IDisposable
    {
#if DEBUG
        public RenderWindow window { get; set; }
#else
        public RenderTexture window { get; set; }
#endif

        protected List<ObjectRender> objects = new();

        protected delegate void UpdateAction(float deltaTime);
        protected UpdateAction TickAction;
        public int CountFrame { get; private set; } = 0;
        public float SpeedRender = 1f;

        public RenderConfig RenderConfig { get; private set; }

        public FPSCounter fpsCounter = new FPSCounter();

        public Color background = Color.Black;

        public virtual List<ActionBase> active { get; set; } = new List<ActionBase>();

        public Render()
        {
            this.RenderConfig = GetDefaultRenderConfig();
            this.objects = new List<ObjectRender>();
        }


        public Render(RenderConfig config, List<ObjectRender> Objects)
        {
            this.RenderConfig = config;
            this.objects = Objects;
        }

        public virtual void Init()
        {
            this.SpeedRender = RenderConfig.SpeedRender;

#if DEBUG
            this.window = new RenderWindow(new VideoMode(RenderConfig.Width, RenderConfig.Height), RenderConfig.Title);
#else
            this.window = new RenderTexture(RenderConfig.Width, RenderConfig.Height);
#endif

            this.window.SetActive(false);
#if DEBUG
            this.window.SetFramerateLimit(RenderConfig.FPS);
#endif

            this.active.ForEach(X => X.SetConfig(RenderConfig));
        }
        public void Start()
        {
            while (Tick())
            {
                fpsCounter.Update();
            }
        }
        private bool ConterWindowRenderActive = false;

        public bool Close = false;

        public bool Tick()
        {
            if (!ConterWindowRenderActive)
            {
                this.window.SetActive(true);
                ConterWindowRenderActive = true;
            }

            if (Close)
                return false;

            float deltaTime = RenderConfig.DeltaTime == TypeDeltaTime.Fixed ? 1.0F / RenderConfig.FPS * SpeedRender : fpsCounter.DeltaTime * SpeedRender;

            this.TickAction?.Invoke(deltaTime);
#if !DEBUG
            window.DispatchEvents();
#endif

            var val = TickRender(deltaTime);
            window.Display();
            CountFrame++;
            return val;
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
            window = null;
        }
    }
}
