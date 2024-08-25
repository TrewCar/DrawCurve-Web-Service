using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Core.Window;
using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.DTO.Models.Objects;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core.Objects;
using Microsoft.Extensions.DependencyInjection;
using SFML.Graphics;
using System.Text.Json;

namespace DrawCurve.Application.Menedgers.Renders
{
    public class MenedgerGenerateFrames : MenedgerRender<Render>
    {
        public MenedgerGenerateFrames(IServiceProvider serviceProvider)
            : base(serviceProvider,
                  search: TypeStatus.ProccessInQueue,
                  proccess: TypeStatus.ProccessRenderFrame,
                  end: TypeStatus.ProccessRenderFrameEnd)
        { }

        public override string Add(int AuthorId, string key)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

                var render = InitRender(queue.GetRender(key));

                render.KEY = key;

                render.OnCompliteRender += OnCompliteRender;
                render.OnDoneFrame += OnDoneFrame;
                Renders.Add(render.KEY, (AuthorId, render));

                Directory.CreateDirectory(DirectoryHelper.GetPathToSaveFrame(render.KEY));

                render.Init();
                render.window.SetActive(false);

                string path = DirectoryHelper.GetPathToSaveFrame(render.KEY);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);
                render.window.SetActive(true);
                render.Start();
                render.window.SetActive(false);
                render.Dispose();
            });

            return key;
        }

        protected async void OnDoneFrame(string key)
        {
            if (!Renders.ContainsKey(key))
                return;

            var render = Renders[key].Value;
            var image = render.window.Capture();
            var path = Path.Combine(DirectoryHelper.GetPathToSaveFrame(key), $"frame_{render.CountFrame:D6}.png");

            await Task.Run(() => image.SaveToFile(path));

            Console.WriteLine($"{key} - {render.CountFrame}");
        }

        protected void OnCompliteRender(string key)
        {
            if (!Renders.ContainsKey(key))
                return;

            Console.WriteLine("END " + key);
            KeyRenderByEnd.Add(key);
        }



        private Render InitRender(RenderInfo render)
        {
            if (render.Type == RenderType.RenderCurve)
            {
                var t = new CurveRender(
                    render.RenderConfig.Transfer(),
                    render.Objects.Select(x => x.Transfer()).ToList()
                );
                t.KEY = render.KEY;
                return t;
            }
            else
            {
                throw new NotImplementedException($"Type {render.Type} is not implemented in {GetType().FullName}");
            }
        }
    }
}
