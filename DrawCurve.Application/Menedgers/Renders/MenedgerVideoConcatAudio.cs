using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrawCurve.Application.Menedgers.Renders
{
    public class MenedgerVideoConcatAudio : MenedgerRender<string>
    {
        public MenedgerVideoConcatAudio(IServiceProvider serviceProvider, ILogger<MenedgerVideoConcatAudio> logger)
            : base(serviceProvider, logger,
                  search: TypeStatus.ProccessConcatFrameEnd,
                  proccess: TypeStatus.ProccessConcatAudio,
                  end: TypeStatus.ProccessEnd)
        { }

        public override string Add(int AuthorId, string Key)
        {
            _ = Task.Run(async () =>
            {
                string _key = (string)Key.Clone();
                string path = DirectoryHelper.GetPathToSaveResult(_key);

                using var scope = _serviceProvider.CreateScope();
                var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
                RenderInfo render = queue.GetRender(_key);

                Renders.Add(Key, (AuthorId, _key));

                string pathToVideo = Path.Combine(DirectoryHelper.GetPathToSaveVideo(_key), _key + ".mp4");

                string pathToAudio = render.RenderConfig.PathMusic;
                if (!string.IsNullOrEmpty(pathToAudio))
                {
                    FFMpegUtils.VideoConcatAudio(
                        pathToVideo: pathToVideo,
                        pathToAudio: render.RenderConfig.PathMusic,
                        pathOutVideo: path,
                        outNameFile: _key);
                } 
                else
                {
                    File.Copy(pathToVideo, Path.Combine(path, _key + ".mp4"), true);
                }

                KeyRenderByEnd.Add(Key);
            });

            return Key;
        }
    }
}
