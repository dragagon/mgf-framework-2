using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Implementation.Messaging;

namespace Servers.Handlers
{
    public class TestRequestEventHandler : IHandler<IClientPeer>
    {
        public MessageType Type { get { return MessageType.Request; } }

        public byte Code { get { return 1; } }

        public int? SubCode { get { return 2; } }

        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            Event evt = new Event(Code, SubCode, new Dictionary<byte, object>());
            peer.SendMessage(evt);
            return true;
        }
    }
}
