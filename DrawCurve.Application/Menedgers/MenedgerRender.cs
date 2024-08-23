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

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerRender
    {
        public static string PathToSaveFrame { get; set; } = Path.Combine(Directory.GetParent(Environment.ProcessPath).FullName, "DataVideo", "Frames");
        public Dictionary<string, (int Author, Render Render)> Renders { get; private set; }
        private List<string> KeyRenderByEnd;

        private IServiceProvider _serviceProvider;

        public MenedgerRender(IServiceProvider serviceProvider)
        {
            Renders = new();
            KeyRenderByEnd = new();
            _serviceProvider = serviceProvider;

            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

            var renders = queue.GetQueue(TypeStatus.ProccessRenderFrame);
            foreach (var item in renders)
            {
                Render render = InitRender(item);
                render.KEY = item.KEY;
                Add(item.AuthorId, render);
            }

            _ = Task.Run(CheckEndAsync);
        }

        public string Add(int AuthorId, Render render)
        {
            render.OnCompliteRender += OnCompliteRender;
            render.OnDoneFrame += OnDoneFrame;
            Renders.Add(render.KEY, (AuthorId, render));

            Directory.CreateDirectory(DirectoryHelper.GetPathToSaveFrame(render.KEY));

            _ = Task.Run(async () =>
            {
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

            return render.KEY;
        }

        protected async void OnDoneFrame(string key)
        {
            if (!Renders.ContainsKey(key))
                return;

            var render = Renders[key].Render;
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

        private async Task CheckEndAsync()
        {
            while (true)
            {
                var tempList = KeyRenderByEnd.ToList();
                List<string> keysToRemove = new();

                foreach (var key in tempList)
                {
                    keysToRemove.Add(key);
                    var render = Renders[key];

                    // No need to call thread.Join(), the task should have already completed
                    Renders.Remove(key);

                    using var scope = _serviceProvider.CreateScope();
                    var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
                    var renderInfo = queue.GetRender(key);
                    queue.UpdateState(renderInfo, TypeStatus.ProccessRenderFrameEnd);
                }

                foreach (var key in keysToRemove)
                {
                    KeyRenderByEnd.Remove(key);

                    using var scope = _serviceProvider.CreateScope();
                    var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
                    var items = queue.GetQueue(TypeStatus.ProccessInQueue);

                    for (int i = 0; i < items.Count - Renders.Count; i++)
                    {
                        queue.UpdateState(items[i], TypeStatus.ProccessRenderFrame);
                        Add(items[i].AuthorId, InitRender(items[i]));
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1)); // wait 1 minute to next iteration
            }
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
                throw new NotImplementedException($"Type {render.Type} is not implemented in {this.GetType().FullName}");
            }
        }
    }
}
