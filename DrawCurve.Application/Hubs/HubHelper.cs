using DrawCurve.Domen.Responces;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DrawCurve.Application.Hubs
{
    public static class HubHelper
    {
        private static IHubContext<TickRenderHub> _hubContext;

        // Метод для инициализации хаба через DI
        public static void Initialize(IHubContext<TickRenderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Статический метод для вызова клиентских методов через хаб
        public static void SendTick(int authorId, RenderTick tick)
        {
            var connections = TickRenderHub._connections.GetConnections(authorId).ToList();
            foreach (var connectionId in connections)
            {
                //Clients.Client(connectionId).SendAsync("tick", (RenderTick)tick);
                _hubContext.Clients.All.SendAsync("tick", tick);
            }
        }
    }

}
