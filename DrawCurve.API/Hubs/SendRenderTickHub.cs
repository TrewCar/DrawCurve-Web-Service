using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace DrawCurve.API.Hubs
{
    [SignalRHub]
    public class SendRenderTickHub : Hub, ISendTickRender
    {
        private readonly MenedgerSession _session;
        private static readonly Dictionary<int, List<string>> _connections = new();

        public SendRenderTickHub(MenedgerSession session)
        {
            _session = session;
        }

        public override async Task OnConnectedAsync()
        {
            User? user = _session.GetUserSession();
            if (user != null)
            {
                if (!_connections.ContainsKey(user.Id))
                {
                    _connections[user.Id] = new List<string>();
                }
                _connections[user.Id].Add(Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            User? user = _session.GetUserSession();
            if (user != null)
            {
                if (_connections.ContainsKey(user.Id))
                {
                    _connections[user.Id].Remove(Context.ConnectionId);
                    if (!_connections[user.Id].Any())
                    {
                        _connections.Remove(user.Id);
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendTick(int userId, RenderTick message)
        {
            if (_connections.ContainsKey(userId))
            {
                var connectionIds = _connections[userId];
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveTickUpdate", message);
                }
            }
        }
    }
}
