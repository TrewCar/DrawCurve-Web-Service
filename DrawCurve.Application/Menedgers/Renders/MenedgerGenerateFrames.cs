﻿using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Core.Window;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.DTO.Models.Objects;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;
using DrawCurve.Domen.Responces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrawCurve.Application.Menedgers.Renders
{
    public class MenedgerGenerateFrames : MenedgerRender<Render>
    {
        public MenedgerGenerateFrames(IServiceProvider serviceProvider, ILogger<MenedgerGenerateFrames> logger)
            : base(serviceProvider, logger,
                  search: TypeStatus.ProccessInQueue,
                  proccess: TypeStatus.ProccessRenderFrame,
                  end: TypeStatus.ProccessRenderFrameEnd)
        { }

        public override string Add(int AuthorId, string key)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var render = InitRender(key);

                    render.KEY = key;

                    render.OnCompliteRender += OnCompliteRender;
                    render.OnDoneFrame += OnDoneFrame;
                    Renders.Add(render.KEY, (AuthorId, render));

                    Directory.CreateDirectory(DirectoryHelper.GetPathToSaveFrame(render.KEY));

                    render.Init();
                    render.window.SetActive(false);

                    string path = DirectoryHelper.GetPathToSaveFrame(render.KEY);
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }

                    Directory.CreateDirectory(path);
                    render.window.SetActive(true);
                    render.Start();
                    render.window.SetActive(false);
                    render.Dispose();
                }
                catch (Exception ex)
                {
                    Error.Add((key, ex.Message));
                }
            });

            return key;
        }

        protected async void OnDoneFrame(string key)
        {
            if (!Renders.ContainsKey(key))
                return;

            var render = Renders[key].Value;
            var image = render.window.Texture.CopyToImage();
            var path = Path.Combine(DirectoryHelper.GetPathToSaveFrame(key), $"frame_{render.CountFrame:D6}.png");

            image.SaveToFile(path);

            await SendTick(Renders[key].Author, new RenderTick
            {
                KeyRender = render.KEY,
                FPS = (float)render.fpsCounter.FPS,
                CountFPS = render.CountFrame,
                MaxCountFPS = render.RenderConfig.FPS * render.RenderConfig.Time,
                Status = proccess
            });

            //Console.WriteLine($"{key} - {render.CountFrame}");
        }

        protected void OnCompliteRender(string key)
        {
            if (!Renders.ContainsKey(key))
                return;

            Console.WriteLine("END " + key);
            KeyRenderByEnd.Add(key);
        }



        private Render InitRender(string key)
        {
            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
            RenderInfo render = queue.GetRender(key);

            if (render.Type == RenderType.RenderCurve)
            {
                var t = new CurveRender(
                    render.RenderConfig.Transfer(),
                    render.Objects.Select(x => x.Transfer()).ToList()
                );
                t.KEY = render.KEY;
                return t;
            }
            else if (render.Type == RenderType.LissajousFigures)
            {
                var t = new LisajuFormsRender(
                    render.RenderConfig.Transfer(),
                    render.Objects.Select(x => x.Transfer()).ToList()
                );
                t.KEY = render.KEY;
                return t;
            }
            else if (render.Type == RenderType.SvgRenderCurve)
            {
                var t = new SvgCurveRender(
                    render.RenderConfig.Transfer(),
                    render.Objects.Select(x => x.Transfer()).ToList()
                );
                t.KEY = render.KEY;
                return t;
            }
            else
            {
                throw new NotImplementedException($"Type {render.Type} is not implemented in {GetType().FullName}");
            }
        }
    }
}
