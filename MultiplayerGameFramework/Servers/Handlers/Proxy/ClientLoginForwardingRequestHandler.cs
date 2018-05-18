using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Config;
using Servers.Data.Client;
using System.Linq;

namespace Servers.Handlers.Proxy
{
    public class ClientLoginForwardingRequestHandler : IHandler<IClientPeer>
    {
        private IClientCodeRemover CodeRemover;
        private ILogger Log;
        private IServerConnectionCollection<IServerType, IServerPeer> ConnectionCollection;

        public ClientLoginForwardingRequestHandler(ILogger log, IClientCodeRemover codeRemover, IServerConnectionCollection<IServerType, IServerPeer> connectionCollection)
        {
            Log = log;
            CodeRemover = codeRemover;
            ConnectionCollection = connectionCollection;
        }

        public MessageType Type => MessageType.Async | MessageType.Request | MessageType.Response;

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => null;

        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            Log.DebugFormat("Received Login Message to Forward");
            var messageForwarded = false;
            // Remove all codes that might attempt to be spoofed by the player
            CodeRemover.RemoveCodes(message);
            Log.DebugFormat("Removed codes from message");

            // Get a list of all appropriate servers - assume only one login server
            var loginServers = ConnectionCollection.GetServersByType<IServerPeer>(ServerType.LoginServer);
            Log.DebugFormat("Found {0} login servers", loginServers.Count);

            // Add in any other data we need before sending the message - the actual peer id of the client, any other data
            AddMessageData(message, peer);

            // Forward the message
            var login = loginServers.FirstOrDefault();
            if (login != null)
            { 
                login.SendMessage(message);
                Log.DebugFormat("Forwarded Message to Login Server");
                messageForwarded = true;
            }
            return messageForwarded;
        }

        private void AddMessageData(IMessage message, IClientPeer peer)
        {
            // ensure the actual peer id of this client is sent foward.
            message.Parameters.Add((byte)MessageParameterCode.PeerId, peer.PeerId.ToByteArray());
            message.Parameters.Add((byte)MessageParameterCode.UserId, peer.ClientData<CharacterData>().UserId);
        }
    }
}
