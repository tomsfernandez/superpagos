using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.Service {
    public class WebSocketManagerMiddleware {
        
        private RequestDelegate Next { get; }
        private WebSocketHandler WebSocketHandler { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
            WebSocketHandler webSocketHandler) {
            Next = next;
            WebSocketHandler = webSocketHandler;
        }
        
        public async Task Invoke(HttpContext context) {
            if (!context.WebSockets.IsWebSocketRequest) return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            WebSocketHandler.OnConnection(socket);

            await Receive(socket, async (result, buffer) => {
                switch (result.MessageType) {
                    case WebSocketMessageType.Text:
                        await WebSocketHandler.ReceiveAsync(socket, result, buffer);
                        break;
                    case WebSocketMessageType.Close:
                        await WebSocketHandler.OnDisconnection(socket);
                        break;
                }
            });

            await Next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage) {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open) {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
    }
}