﻿using DrawCurve.Application.Interface;
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
                try
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



                    var directoryPath = DirectoryHelper.GetPathToSaveFrame(_key);
                    var files = Directory.GetFiles(directoryPath, "frame_*.png").OrderBy(f => f).ToList();
                    if (files.Count > 0)
                    {
                        var middleIndex = files.Count / 2;
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (i != middleIndex)
                            {
                                File.Delete(files[i]);
                            }
                        }
                    }
                    KeyRenderByEnd.Add(Key);
                }
                catch (Exception ex)
                {
                    Error.Add((Key, ex.Message));
                }
            });

            return Key;
        }
    }
}
