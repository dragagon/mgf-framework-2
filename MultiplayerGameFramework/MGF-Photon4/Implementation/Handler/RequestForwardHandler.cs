using System;
using ExitGames.Logging;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;

namespace MGF_Photon.Implementation.Handler
{
    public class RequestForwardHandler : ServerHandler, IDefaultRequestHandler<IServerPeer>
    {
        private readonly IConnectionCollection<IClientPeer> _connectionCollection;
        private readonly ServerConfiguration _serverConfiguration;

        public ILogger Log { get; set; }

        public RequestForwardHandler(ILogger log, IConnectionCollection<IClientPeer> connectionCollection, ServerConfiguration serverConfiguration)
        {
            Log = log;
            _connectionCollection = connectionCollection;
            _serverConfiguration = serverConfiguration;
        }

        public override MessageType Type
        {
            get
            {
                return MessageType.Request;
            }
        }

        public override byte Code
        {
            get
            {
                return 0x0ff;
            }
        }

        public override int? SubCode
        {
            get
            {
                return null;
            }
        }

        public override bool OnHandleMessage(IMessage message, IServerPeer serverPeer)
        {
            Log.DebugFormat("No Existing Request Handler - RequestForwardHandler");
            return true;
        }
    }
}
