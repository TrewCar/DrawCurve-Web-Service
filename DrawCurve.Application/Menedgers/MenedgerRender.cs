using DrawCurve.Core.Window;
using System.Reflection;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerRender
    {
        public Dictionary<string, Render> Renders { get; private set; }
        private Dictionary<string, Thread> threads;

        private List<string> KeyTreathByEnd;
        private Thread _threadKeyTreathByEnd;

        public MenedgerRender()
        {
            Renders = new();
            threads = new();
            KeyTreathByEnd = new();
            _threadKeyTreathByEnd = new Thread(CheckEnd);
            _threadKeyTreathByEnd.Start();
            Directory.CreateDirectory("test");
        }

        public string Add(Render render)
        {
            render.OnCompliteRender += OnCompliteRender;
            render.OnDoneFrame += OnDoneFrame;
            Renders.Add(render.KEY, render);
            Directory.CreateDirectory(Path.Combine("test", render.KEY));
            var thread = new Thread(() =>
            {
                render.Init();
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

            Renders[key].window.Texture.CopyToImage()
                .SaveToFile(Path.Combine("test", Renders[key].KEY, Renders[key].KEY + "_" + Renders[key].CountFrame + ".png"));

            Console.WriteLine(key + " - " + Renders[key].CountFrame);
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
                //Thread.Sleep(1000);
            }
        }
    }

}
