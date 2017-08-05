using MultiplayerGameFramework.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Messaging;

namespace MultiplayerGameFramework.Implementation.Client
{
    public class SubServerClientPeer : IClientPeer
    {
        public bool IsProxy { get; set; }
        public Guid PeerId { get; set; }
        private readonly Dictionary<Type, IClientData> _clientData;

        public delegate SubServerClientPeer ClientPeerFactory();

        public SubServerClientPeer(IEnumerable<IClientData> clientData)
        {
            _clientData = new Dictionary<Type, IClientData>();
            foreach(var data in clientData)
            {
                _clientData.Add(data.GetType(), data);
            }
        }

        public void Disconnect()
        {
            // Don't do anything with this
        }

        public void SendMessage(IMessage message)
        {
            // Never send anything
        }

        public T ClientData<T>() where T : class, IClientData
        {
            IClientData result;
            _clientData.TryGetValue(typeof(T), out result);
            if(result != null)
            {
                return result as T;
            }
            return null;
        }
    }
}
