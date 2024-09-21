using DrawCurve.Application.Interface;
using DrawCurve.Domen.Responces;
using Microsoft.AspNetCore.SignalR;
using SFML.Window;

namespace DrawCurve.API.Hubs
{
    public class TickRenderHub : Hub, ISendTickRender
    {
        private static readonly ConnectionMapping<int> _connections = new ConnectionMapping<int>();

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserIdFromContext();
            if (userId.HasValue)
            {
                _connections.Add(userId.Value, Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetUserIdFromContext();
            if (userId.HasValue)
            {
                _connections.Remove(userId.Value, Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendTick(int authorId, RenderTick tick)
        {
            var connections = _connections.GetConnections(authorId) ?? new List<string>();
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("tick", tick);
            }
        }

        private int? GetUserIdFromContext()
        {
            var userIdClaim = Context.User?.FindFirst("Id");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
        }
    }

}
