using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Web.Service {
    public class EchoWebSocketHandler : WebSocketHandler{
        public EchoWebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) 
            : base(webSocketConnectionManager) { }
        
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer) {
            var message = Encoding.UTF8.GetString(buffer);
            await SendMessageAsync(socket, message);
        }
    }
}