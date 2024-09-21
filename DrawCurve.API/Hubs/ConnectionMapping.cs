namespace DrawCurve.API.Hubs
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out var connections))
                {
                    connections = new HashSet<string>();
                    _connections[key] = connections;
                }
                connections.Add(connectionId);
            }
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                if (_connections.TryGetValue(key, out var connections))
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            lock (_connections)
            {
                return _connections.TryGetValue(key, out var connections) ? connections : Enumerable.Empty<string>();
            }
        }
    }

}
