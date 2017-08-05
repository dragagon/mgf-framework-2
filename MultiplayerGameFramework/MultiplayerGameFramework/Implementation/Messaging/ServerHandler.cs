using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Messaging
{
    public abstract class ServerHandler : IHandler<IServerPeer>
    {
        public abstract byte Code { get; }
        public abstract int? SubCode { get; }
        public abstract MessageType Type { get; }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            return OnHandleMessage(message, peer);
        }

        public abstract bool OnHandleMessage(IMessage message, IServerPeer peer);

    }
}
