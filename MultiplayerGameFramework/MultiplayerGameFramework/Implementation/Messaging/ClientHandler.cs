using System;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Client;

namespace MultiplayerGameFramework.Implementation.Messaging
{
    public abstract class ClientHandler : IHandler<IClientPeer>
    {
        public abstract byte Code { get; }
        public abstract int? SubCode { get; }
        public abstract MessageType Type { get; }

        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            return OnHandleMessage(message, peer);
        }

        public abstract bool OnHandleMessage(IMessage message, IClientPeer peer);
    }
}
