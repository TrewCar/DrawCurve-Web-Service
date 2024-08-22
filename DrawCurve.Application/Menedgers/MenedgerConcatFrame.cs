using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Domen.Core.Menedger.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerConcatFrame
    {
        public Dictionary<string, (int Author, string Key)> Renders { get; private set; } = new();
        private Dictionary<string, Thread> threads = new();

        private List<string> KeyTreathByEnd = new();

        private IServiceProvider _serviceProvider;
        private Thread _threadKeyTreathByEnd;
        public MenedgerConcatFrame(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

            var renders = queue.GetQueue(TypeStatus.ProccessConcatFrame);
            foreach (var item in renders)
            {
                this.Add(item.AuthorId, item.KEY);
            }

            _threadKeyTreathByEnd = new Thread(CheckEnd);
            _threadKeyTreathByEnd.Start();
        }
        public string Add(int AuthorId, string Key)
        {
            Renders.Add(Key, (AuthorId, Key));
            var thread = new Thread(() =>
            {
                string _key = (string)Key.Clone();
                string path = DirectoryHelper.GetPathToSaveVideo(_key);
                if(Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);

                using var scope = _serviceProvider.CreateScope();
                var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

                var render = queue.GetRender(Key);

                string pathToFrame = DirectoryHelper.GetPathToSaveFrame(_key);

                FFMpegUtils.ConcatFrames(
                    FPS:            render.RenderConfig.FPS, 
                    paternFrames:   "frame_%06d.png",
                    pathToFrames:   pathToFrame, 
                    pathOutVideo:   path,
                    fileName:       _key);

                KeyTreathByEnd.Add(Key);
            });
            //thread.UnsafeStart();
            thread.Start();

            threads.Add(Key, thread);

            return Key;
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

                    queue.UpdateState(renderInfo, TypeStatus.ProccessConcatFrameEnd);
                }

                if (Renders.Count < 10)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

                    var items = queue.GetQueue(TypeStatus.ProccessRenderFrameEnd);

                    for (int i = 0; i < items.Count - Renders.Count; i++)
                    {
                        queue.UpdateState(items[i], TypeStatus.ProccessConcatFrame);
                        this.Add(items[i].AuthorId, items[i].KEY);
                    }
                }

                Thread.Sleep(1000 * 60); // wait 1 minutes to next
            }
        }
    }
}
