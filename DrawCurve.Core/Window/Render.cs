using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using SFML.Graphics;

namespace DrawCurve.Core.Window
{
    public abstract class Render : IDisposable
    {
        public RenderWindow window { get; set; }

        protected delegate void UpdateAction(float deltaTime);
        protected UpdateAction TickAction;
        public int CountFrame { get; private set; } = 0;
        public float SpeedRender = 1f;

        public RenderConfig RenderConfig { get; private set; }

        public FPSCounter fpsCounter = new FPSCounter();

        public Color background = Color.Black;

        public virtual List<ActionBase> active { get; set; } = new List<ActionBase>();

        public Render()
            => Init(GetDefaultRenderConfig());
        

        public Render(RenderConfig config)
            => Init(config);
        
        private void Init(RenderConfig config)
        {
            this.RenderConfig = config;
            this.SpeedRender = config.SpeedRender;

            this.window = new RenderWindow(RenderConfig.VideoMode, RenderConfig.Title);
            this.window.SetActive(false);

            //this.window.SetFramerateLimit(RenderConfig.FPS);

            this.active.ForEach(X => X.SetConfig(RenderConfig));
        }
        public void Start()
        {
            while (Tick())
            {
                CountFrame++;
                fpsCounter.Update();
            }
        }
        private bool ConterWindowRenderActive = false;
        public bool Tick()
        {
            if (!ConterWindowRenderActive)
            {
                this.window.SetActive(true);
                ConterWindowRenderActive = true;
            }

            if (!window.IsOpen)
                return false;

            float deltaTime = RenderConfig.DeltaTime == TypeDeltaTime.Fixed ? 1.0F / RenderConfig.FPS * SpeedRender : fpsCounter.DeltaTime * SpeedRender;

            this.TickAction?.Invoke(deltaTime);

            window.DispatchEvents();

            var val = TickRender(deltaTime);
            window.Display();
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
