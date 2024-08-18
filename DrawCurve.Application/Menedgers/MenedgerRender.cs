using DrawCurve.Application.Interface;
using DrawCurve.Application.Services;
using DrawCurve.Core.Window;
using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.DTO.Models.Objects;
using DrawCurve.Domen.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SFML.Graphics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerRender
    {
        public static readonly string PathToSaveFrame = Path.Combine("DateVideo", "Frames");
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
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*.png");
                    if (files.Length > 10 && File.Exists(Path.Combine(PathToSaveFrame, render.KEY, "NOW_STATE.json")))
                    {
                        Texture texture = new Texture(files.Last());
                        render.window.Texture.Swap(texture);
                    }
                    else
                    {
                        Directory.Delete(path, true);
                        Directory.CreateDirectory(path);
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                //render.window.SetActive(true);
                render.Start();
                //render.window.SetActive(false);
                render.Dispose();
            });
            //thread.UnsafeStart();
            thread.Start();

            threads.Add(render.KEY, thread);

            return render.KEY;
        }

        protected void OnDoneFrame(string key)
        {
            if (!Renders.ContainsKey(key))
                return;

            Renders[key].Render.window.Texture.CopyToImage()
                .SaveToFile(Path.Combine(PathToSaveFrame, Renders[key].Render.KEY, "frame_" + Renders[key].Render.CountFrame + ".png"));

            // Сохраняем текущие состояние 
            var json = JsonConvert.SerializeObject(
                Renders[key].Render.Objects.Select(x => x.Transfer()),
                Formatting.Indented,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            File.Delete(Path.Combine(PathToSaveFrame, Renders[key].Render.KEY, "NOW_STATE.json"));
            File.WriteAllText(Path.Combine(PathToSaveFrame, Renders[key].Render.KEY, "NOW_STATE.json"), json);

            Console.WriteLine(key + " - " + Renders[key].Render.CountFrame);
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
                }

                if(Renders.Count < 10)
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
                return new CurveRender(
                    render.RenderConfig.Transfer(),
                    render.Objects.Select(x => x.Transfer()).ToList()
                );
            }
            else
            {
                throw new NotImplementedException($"Type {render.Type} is not implementad in {this.GetType().FullName}");
            }
        }
    }
}
