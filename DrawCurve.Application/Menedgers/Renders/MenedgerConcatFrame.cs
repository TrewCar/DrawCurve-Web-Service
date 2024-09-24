using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace DrawCurve.Application.Menedgers.Renders
{
    public class MenedgerConcatFrame : MenedgerRender<string>
    {
        public MenedgerConcatFrame(IServiceProvider serviceProvider, ILogger<MenedgerConcatFrame> logger)
            : base(serviceProvider, logger,
                  search: TypeStatus.ProccessRenderFrameEnd,
                  proccess: TypeStatus.ProccessConcatFrame,
                  end: TypeStatus.ProccessConcatFrameEnd)
        { }
        public override string Add(int AuthorId, string Key)
        {
            _ = Task.Run(async () =>
            {
                string _key = (string)Key.Clone();
                string path = DirectoryHelper.GetPathToSaveVideo(_key);

                Renders.Add(Key, (AuthorId, _key));

                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);

                using var scope = _serviceProvider.CreateScope();
                var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

                var render = queue.GetRender(Key);

                string pathToFrame = DirectoryHelper.GetPathToSaveFrame(_key);

                FFMpegUtils.ConcatFrames(
                    FPS: (uint)render.RenderConfig.FPS,
                    paternFrames: "frame_%06d.png",
                    pathToFrames: pathToFrame,
                    pathOutVideo: path,
                    outNameFile: _key);

                KeyRenderByEnd.Add(Key);


                Directory.Delete(DirectoryHelper.GetPathToSaveFrame(_key), true);
            });

            return Key;
        }
    }
}
