using DrawCurve.Application.Interface;
using DrawCurve.Core.Window;
using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.DTO.Models.Objects;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core.Objects;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using SFML.Graphics;
using System.Text.Json;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerRender
    {
        public static readonly string PathToSaveFrame = Path.Combine(Directory.GetParent(Environment.ProcessPath).FullName, "DateVideo", "Frames");
        public Dictionary<string, (int Author, Render Render)> Renders { get; private set; }
        private Dictionary<string, Thread> threads;

        private List<string> KeyTreathByEnd;
        private Thread _threadKeyTreathByEnd;

        private IServiceProvider _serviceProvider;

        public MenedgerRender(IServiceProvider serviceProvider)
        {
            Renders = new();
            threads = new();
            KeyTreathByEnd = new();

            this._serviceProvider = serviceProvider;

            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

            var renders = queue.GetBroken();
            foreach (var item in renders)
            {
                Render render = this.InitRender(item);

                render.KEY = item.KEY;

                this.Add(item.AuthorId, render);
            }
            _threadKeyTreathByEnd = new Thread(CheckEnd);
            _threadKeyTreathByEnd.Start();
            Directory.CreateDirectory("test");
        }

        public string Add(int AuthorId, Render render)
        {
            render.OnCompliteRender += OnCompliteRender;
            render.OnDoneFrame += OnDoneFrame;
            Renders.Add(render.KEY, (AuthorId, render));
            Directory.CreateDirectory(Path.Combine(PathToSaveFrame, render.KEY));
            var thread = new Thread(() =>
            {
                render.Init();
                string path = Path.Combine(PathToSaveFrame, render.KEY);
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
                render.window.SetActive(true);
                render.Start();
                render.window.SetActive(false);
                render.Dispose();
            });
            //thread.UnsafeStart();
            thread.Start();

            threads.Add(render.KEY, thread);

            return render.KEY;
        }

        protected async void OnDoneFrame(string key)
        {
            if (!Renders.ContainsKey(key))
                return;
            var render = Renders[key].Render;

            var image = render.window.Texture.CopyToImage();
            var path = Path.Combine(PathToSaveFrame, key, $"frame_{render.CountFrame:D6}.png");

            await Task.Run(() => image.SaveToFile(path));

            Console.WriteLine(key + " - " + render.CountFrame);
        }


        protected void OnCompliteRender(string key)
        {
            if (!threads.ContainsKey(key))
                return;
            Console.WriteLine("END " + key);

            KeyTreathByEnd.Add(key);
        }
        /// <summary>
        /// Условный костыль, позволяющий закрыть поток рендера вне его потока
        /// </summary>
        private void CheckEnd()
        {
            while (true)
            {
                var tempList = KeyTreathByEnd.ToList();
                List<string> keysRemve = new();
                for (int i = 0; i < tempList.Count; i++)
                {
                    string KEY = KeyTreathByEnd[i];
                    keysRemve.Add(KEY);


                    var thread = threads[KEY];
                    var render = Renders[KEY];

                    thread.Join();
                    //render.Dispose();
                    Renders.Remove(KEY);
                    threads.Remove(KEY);
                }
                foreach (var key in keysRemve)
                {
                    KeyTreathByEnd.Remove(key);

                    using var scope = _serviceProvider.CreateScope();
                    var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
                    var renderInfo = queue.GetRender(key);

                    queue.UpdateState(renderInfo, TypeStatus.ProccessConcatFrame);
                }

                if (Renders.Count < 10)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

                    var items = queue.GetQueue();

                    for (int i = 0; i < items.Count - Renders.Count; i++)
                    {
                        queue.UpdateState(items[i], TypeStatus.ProccessRenderFrame);
                        this.Add(items[i].AuthorId, this.InitRender(items[i]));
                    }
                }

                Thread.Sleep(1000 * 60); // wait 1 minutes to next
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
                throw new NotImplementedException($"Type {render.Type} is not implementad in {this.GetType().FullName}");
            }
        }
    }
}
