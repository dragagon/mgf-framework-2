using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Client
{
    public class ClientConnectionCollection : IConnectionCollection<IClientPeer>
    {
        private readonly IEnumerable<IClientConnectionHandler> _clientConnectionHandlers;
        private List<IClientPeer> _clients;

        public ClientConnectionCollection(IEnumerable<IClientConnectionHandler> clientConnectionHandlers)
        {
            _clientConnectionHandlers = clientConnectionHandlers;
            _clients = new List<IClientPeer>();
        }

        public void Clear()
        {
            _clients.Clear();
        }

        public void Connect(IClientPeer peer)
        {
            _clients.Add(peer);
            foreach(var clientConnectionHandler in _clientConnectionHandlers)
            {
                clientConnectionHandler.ClientConnect(peer);
            }
        }

        public void Disconnect(IClientPeer peer)
        {
            _clients.Remove(peer);
            foreach(var clientConnectionHandler in _clientConnectionHandlers)
            {
                clientConnectionHandler.ClientDisconnect(peer);
            }
        }

        List<T> IConnectionCollection<IClientPeer>.GetPeers<T>()
        {
            return new List<T>(_clients.Cast<T>());
        }
    }
}
