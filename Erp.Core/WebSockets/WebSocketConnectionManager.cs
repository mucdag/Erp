using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.WebSockets
{
    public class ByzWebSocket { public string PersonId { get; set; } public string GuidId { get; set; } }
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<ByzWebSocket, WebSocket> _sockets = new ConcurrentDictionary<ByzWebSocket, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key.GuidId == id).Value;
        }

        public List<WebSocket> GetSocketByPersonId(string id)
        {
            return _sockets.Where(p => p.Key.GuidId == id).Select(x => x.Value).ToList();
        }


        public ConcurrentDictionary<ByzWebSocket, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key.GuidId;
        }

        public string GetPersonId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key.PersonId;
        }

        public void AddSocket(WebSocket socket, string personId = null)
        {
            string sId = CreateConnectionId();
            while (!_sockets.TryAdd(new ByzWebSocket { GuidId = sId, PersonId = personId }, socket))
            {
                sId = CreateConnectionId();
            }

        }

        public async Task RemoveSocketByGuidId(string id)
        {
            try
            {
                var socket = _sockets.FirstOrDefault(p => p.Key.GuidId == id);
                var value = socket.Value;
                _sockets.TryRemove(socket.Key, out value);

                await value.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            }
            catch (Exception)
            {

            }

        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
