using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Service {
    public class WebSocketConnectionManager {
        private ConcurrentDictionary<string, WebSocket> Sockets { get; }

        public WebSocketConnectionManager() {
            Sockets = new ConcurrentDictionary<string, WebSocket>();
        }

        public WebSocket GetSocketById(string id) {
            return Sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll() {
            return Sockets;
        }

        public string GetId(WebSocket socket) {
            return Sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket) {
            Sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id) {
            WebSocket socket;
            Sockets.TryRemove(id, out socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the WebSocketManager",
                cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId() {
            return Guid.NewGuid().ToString();
        }
    }
}