using DrawCurve.Application.Utils;
using DrawCurve.Domen.Core.Menedger.Models;
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

                Renders.Add(Key, (AuthorId, _key));

                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);


                string pathToVideo = Path.Combine(DirectoryHelper.GetPathToSaveVideo(_key), _key + ".mp4");

                if (Directory.Exists(DirectoryHelper.PathToAudio()))
                {
                    var files = Directory.GetFiles(DirectoryHelper.PathToAudio());
                    if (files.Length > 0)
                    {
                        FFMpegUtils.VideoConcatAudio(
                            pathToVideo: pathToVideo,
                            pathToAudio: "",
                            pathOutVideo: path,
                            outNameFile: _key);
                    }
                }

                KeyRenderByEnd.Add(Key);
            });

            return Key;
        }
    }
}
