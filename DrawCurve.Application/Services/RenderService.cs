using DrawCurve.Application.Interface;
using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.Models;
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
        public RenderService(DrawCurveDbContext context)
        {
            this.context = context;
        }

        public RenderInfo? GetRender(string Key)
        {
            return this.context.RenderInfo.Where(x => x.KEY == Key).FirstOrDefault();
        }

        public void Queue(RenderInfo queue)
        {
            context.RenderInfo.Add(queue);

            context.SaveChanges();
        }

        public List<RenderInfo> GetBroken()
        {
            return context.RenderInfo
                .Where(x => x.Status == TypeStatus.ProccessRenderFrame)
                .ToList();
        }

        public List<RenderInfo> GetQueue()
        {
            return context.RenderInfo
                .Where(x => x.Status == TypeStatus.ProccessInQueue)
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
