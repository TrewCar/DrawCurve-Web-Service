using DrawCurve.Application.Config;
using DrawCurve.Application.Interface;
using DrawCurve.Application.Services;
using DrawCurve.Application.Utils;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DrawCurve.Application.Menedgers.Renders
{
    public class MenedgerVideoConcatAudio : MenedgerRender<string>
    {
        private readonly FFMpegService FFMpegService;
        public MenedgerVideoConcatAudio(IServiceProvider serviceProvider, ILogger<MenedgerGenerateFrames> logger, IOptions<RenderApplicationConfig> cnf, FFMpegService FFMpegService)
            : base(serviceProvider, logger,
                  maxQueue: cnf.Value.QueueOptions.MaxProccessConcatAudio,
                  search: TypeStatus.ProccessConcatFrameEnd,
                  proccess: TypeStatus.ProccessConcatAudio,
                  end: TypeStatus.ProccessEnd)
        {
            this.FFMpegService = FFMpegService;
        }

        public override string Add(int AuthorId, string Key)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    string _key = (string)Key.Clone();
                    string path = DirectoryHelper.GetPathToSaveResult(_key);

                    using var scope = _serviceProvider.CreateScope();
                    var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
                    RenderInfo render = queue.GetRender(_key);
                    string pathToVideo = Path.Combine(DirectoryHelper.GetPathToSaveVideo(_key), _key + ".mp4");
                    Renders.Add(Key, (AuthorId, _key));
                    try
                    {
                        string pathToAudio = render.RenderConfig.PathMusic;
                        if (!string.IsNullOrEmpty(pathToAudio))
                        {
                            FFMpegService.VideoConcatAudio(
                                pathToVideo: pathToVideo,
                                pathToAudio: render.RenderConfig.PathMusic,
                                pathOutVideo: path,
                                outNameFile: _key);
                        }
                        else
                        {
                            File.Copy(pathToVideo, Path.Combine(path, _key + ".mp4"), true);
                        }
                    }
                    catch (Exception ex)
                    {
                        File.Copy(pathToVideo, Path.Combine(path, _key + ".mp4"), true);
                    }

                    KeyRenderByEnd.Add(Key);

                    Directory.Delete(DirectoryHelper.GetPathToSaveVideo(_key), true);
                }
                catch (Exception ex) {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            });

            return Key;
        }
    }
}
