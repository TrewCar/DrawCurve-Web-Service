using DrawCurve.Application.Interface;
using DrawCurve.Application.Menedgers;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;
using DrawCurve.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Services
{
    public class RenderService : IRenderService, IRenderQueue
    {
        private readonly DrawCurveDbContext context;
        private CheckLiminters limiter;
        public RenderService(DrawCurveDbContext context, CheckLiminters limiter)
        {
            this.context = context;
            this.limiter = limiter;
        }
        public List<RenderInfo> GetRenderList(int id)
        {
            return this.context.RenderInfo
                .Where(x => x.AuthorId == id)
                .OrderByDescending(x => x.DateCreate)
                .ToList();
        }
        public RenderInfo? GetRender(string Key)
        {
            return this.context.RenderInfo.Where(x => x.KEY == Key).FirstOrDefault();
        }

        public void Queue(RenderInfo queue)
        {
            var listSets = limiter.CheckConfig(ref queue);

            context.RenderInfo.Add(queue);

            context.SaveChanges();
        }

        public List<RenderInfo> GetQueue(TypeStatus status)
        {
            return context.RenderInfo
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.DateCreate)
                .Take(10)
                .ToList();
        }

        public void UpdateState(RenderInfo render, TypeStatus status)
        {
            render.Status = status;
            context.RenderInfo.Update(render);
            context.SaveChanges();
        }
    }
}
