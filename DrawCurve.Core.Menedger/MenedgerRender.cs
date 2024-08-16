using DrawCurve.Core.Window;
using DrawCurve.Domen.Core.Menedger.Models;

namespace DrawCurve.Core.Menedger
{
    public class MenedgerRender
    {
        public Dictionary<string, Render> Renders { get; private set; }
        private Dictionary<string, Thread> threads;

        private List<string> KeyTreathByEnd;
        private Thread _threadKeyTreathByEnd;

        public MenedgerRender()
        {
            this.Renders = new();
            this.threads = new();
            this.KeyTreathByEnd = new();
            _threadKeyTreathByEnd = new Thread(CheckEnd);
        }

        public string Add(Render render)
        {
            render.OnCompliteRender += OnCompliteRender;
            render.OnDoneFrame += OnDoneFrame;
            Renders.Add(render.KEY, render);

            var thread = new Thread(() =>
            {
                render.Init();
                render.window.SetActive(true);
                render.Start();
                render.window.SetActive(false);
                render.Dispose();
            });
            thread.UnsafeStart();
            //thread.Start();

            threads.Add(render.KEY, thread);

            return render.KEY;
        }

        protected void OnDoneFrame(string key)
        {
            if (!Renders.ContainsKey(key))
                return;
            Console.WriteLine(key);
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
                List<string> keysRemve = new();
                for (int i = 0; i < KeyTreathByEnd.Count; i++)
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
                keysRemve.ForEach(key => KeyTreathByEnd.Remove(key));
            }
        }
    }

}
