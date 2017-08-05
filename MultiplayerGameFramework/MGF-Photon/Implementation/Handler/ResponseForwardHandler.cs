using ExitGames.Logging;
using MGF_Photon.Implementation.Client;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF_Photon.Implementation.Handler
{
    public class ResponseForwardHandler : ServerHandler, IDefaultResponseHandler<IServerPeer>
    {
        private readonly IConnectionCollection<IClientPeer> _connectionCollection;
        private readonly ServerConfiguration _serverConfiguration;
        private readonly IClientCodeRemover _clientCodeRemover;
        public ILogger Log { get; set; }

        public ResponseForwardHandler(ILogger log, IConnectionCollection<IClientPeer> connectionCollection, ServerConfiguration serverConfiguration, IClientCodeRemover clientCodeRemover)
        {
            Log = log;
            _connectionCollection = connectionCollection;
            _serverConfiguration = serverConfiguration;
            _clientCodeRemover = clientCodeRemover;
        }

        public override MessageType Type
        {
            get
            {
                return MessageType.Response;
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
            Log.DebugFormat("Forwarding Response {0}-{1}", message.Code, message.SubCode);
            if(message.Parameters.ContainsKey(_serverConfiguration.PeerIdCode))
            {
                Log.DebugFormat("Looking for Peer Id {0}", new Guid((Byte[])message.Parameters[_serverConfiguration.PeerIdCode]));
                PhotonClientPeer peer = _connectionCollection.GetPeers<PhotonClientPeer>().FirstOrDefault(p => p.PeerId == new Guid((Byte[])message.Parameters[_serverConfiguration.PeerIdCode]));
                if(peer != null)
                {
                    Log.DebugFormat("Found Peer");

                    message.Parameters.Remove(_serverConfiguration.PeerIdCode);
                    _clientCodeRemover.RemoveCodes(message);

                    var response = message as Response;
                    if(response != null)
                    {
                        peer.SendOperationResponse(
                            new OperationResponse(response.Code, response.Parameters)
                            {
                                DebugMessage = response.DebugMessage,
                                ReturnCode = response.ReturnCode
                            },
                            new SendParameters());
                        
                    }
                    else
                    {
                        peer.SendOperationResponse(new OperationResponse(message.Code, message.Parameters), new SendParameters());
                    }
                }
            }
            return true;
        }
    }
}
