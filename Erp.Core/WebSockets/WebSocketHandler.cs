using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.WebSockets
{
    public abstract class WebSocketHandler
    {
        public static WebSocketConnectionManager _webSocketConnectionManager { get; set; }

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            _webSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual void OnConnected(WebSocket socket)
        {
            _webSocketConnectionManager.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await _webSocketConnectionManager.RemoveSocketByGuidId(_webSocketConnectionManager.GetId(socket));
        }

        public static async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public static async Task SendMessageAsyncByGuidId(string socketId, string message)
        {
            try
            {
                await SendMessageAsync(_webSocketConnectionManager.GetSocketById(socketId), message);
            }
            catch (Exception)
            {

            }

        }

        public static async Task SendMessageAsyncByPersonId(string personId, string message)
        {
            try
            {
                foreach (var socket in _webSocketConnectionManager.GetSocketByPersonId(personId))
                {
                    await SendMessageAsync(socket, message);
                }

            }
            catch (Exception)
            {

            }

        }

        public static async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in _webSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
