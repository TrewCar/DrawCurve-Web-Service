using DrawCurve.Application.Interface;
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
        public RenderService(DrawCurveDbContext context)
        {
            this.context = context;
        }
        public List<RenderInfo> GetRenderList(User user)
        {
            return this.context.RenderInfo
                .Where(x => x.AuthorId == user.Id)
                .OrderByDescending(x => x.DateCreate)
                .ToList();
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
