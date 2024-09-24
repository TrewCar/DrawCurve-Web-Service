using DrawCurve.Application.Interface;
using DrawCurve.Application.Utils;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Responces;
using DrawCurve.Infrastructure;

namespace DrawCurve.Application.Services
{
    public class VideoService : IVideoService
    {
        private readonly DrawCurveDbContext context;
        private readonly IRenderService renderService;
        private readonly IRenderQueue renderQueue;

        public VideoService(DrawCurveDbContext context, IRenderService renderService, IRenderQueue renderQueue)
        {
            this.context = context;
            this.renderService = renderService;
            this.renderQueue = renderQueue;
        }

        public VideoInfo? GetInfo(string Key)
        {
            return this.context.VideoInfo.Where(X => X.RenderCnfId == Key).First() ?? null;
        }

        public string GetVideo(string Key)
        {
            var video = this.GetInfo(Key);
            if (video == null)
            {
                return string.Empty;
            }

            return Path.Combine(DirectoryHelper.GetPathToSaveResult(Key), Key + ".mp4");
        }

        public VideoInfo? Publish(VideoResponce responce, int AuthorId)
        {
            var render = renderService.GetRender(responce.RenderCnfId);

            if (render == null)
                return null;

            if (render.Status != Domen.Models.Menedger.TypeStatus.ProccessEnd)
                return null;



            var videoInfo = new VideoInfo()
            {
                RenderCnfId = responce.RenderCnfId,
                AuthorId = AuthorId,
                Name = responce.Name,
                Description = responce.Description,
            };

            this.context.VideoInfo.Add(videoInfo);
            renderQueue.UpdateState(render, Domen.Models.Menedger.TypeStatus.Publish);
            this.context.SaveChanges();

            return videoInfo;
        }
    }
}
