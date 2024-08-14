using DrawCurve.Core.Window;
using DrawCurve.Domen.Core.Menedger.Models;

namespace DrawCurve.Core.Menedger
{
    public class MenedgerRender
    {
        public Render render;

        public delegate void SendStatusRender(RenderTick status);
        public SendStatusRender OnSendStatusRender;

        public delegate void SendEndProccess();
        public SendEndProccess OnEndProccess;

        public MenedgerRender(Render render)
        {
            this.render = render;
        }
        public void Init()
        {
            this.render.Init();
        }
        public void Start()
        {
            while (!this.render.Close)
            {
                this.render.Tick();
                if (this.render.RenderConfig.FPS * this.render.RenderConfig.Time <= this.render.CountFrame){
                    this.render.Close = true;
                }
                //var image = this.render.window.Capture();
                var status = new RenderTick()
                {
                    FPS = (float)this.render.fpsCounter.FPS,
                    CountFPS = this.render.CountFrame,
                    MaxCountFPS = this.render.RenderConfig.FPS * this.render.RenderConfig.Time,
                    Status = TypeStatus.ProccessRenderFrame,
                };
                this.OnSendStatusRender?.Invoke(status);
            }
            this.OnEndProccess?.Invoke();
        }
        public void Dispose()
        {
            this.render.Close = true;
            this.render.Dispose();
        }
    }
}
