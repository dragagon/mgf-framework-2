using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Handlers.Proxy
{
    public class LoginAuthenticationResponseHandler : IHandler<IServerPeer>
    {

        public MessageType Type => MessageType.Response;

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.LoginUserPass;

        public ILogger Log { get; private set; }

        private readonly IConnectionCollection<IClientPeer> _connectionCollection;
        private readonly ServerConfiguration _serverConfiguration;
        private readonly IClientCodeRemover _clientCodeRemover;

        public LoginAuthenticationResponseHandler(ILogger log, IConnectionCollection<IClientPeer> connectionCollection, ServerConfiguration serverConfiguration, IClientCodeRemover clientCodeRemover)
        {
            Log = log;
            _connectionCollection = connectionCollection;
            _serverConfiguration = serverConfiguration;
            _clientCodeRemover = clientCodeRemover;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            // Got a response back from the LoginUserPass - might be successful, might not.
            if (message.Parameters.ContainsKey(_serverConfiguration.PeerIdCode))
            {
                Log.DebugFormat("Looking for Peer Id {0}", new Guid((Byte[])message.Parameters[_serverConfiguration.PeerIdCode]));
                IClientPeer clientPeer = _connectionCollection.GetPeers<IClientPeer>().FirstOrDefault(p => p.PeerId == new Guid((Byte[])message.Parameters[_serverConfiguration.PeerIdCode]));
                if (clientPeer != null)
                {
                    Log.DebugFormat("Found Peer");

                    var response = message as Response;
                    // copy our response to a return response
                    Response returnResponse = new Response(Code, SubCode, message.Parameters);
                    // remove any unnecessary codes from the returning packet
                    _clientCodeRemover.RemoveCodes(returnResponse);

                    if (response.ReturnCode == (short)ReturnCode.OK)
                    {
                        // Good response, get the client data and look for the userId to set it for the future.
                        clientPeer.ClientData<CharacterData>().UserId = (int)response.Parameters[(byte)MessageParameterCode.UserId];
                    }
                    // make one call to send the message back - One "exit point" for the message.
                    clientPeer.SendMessage(response);
                }
            }
            return true;
        }
    }
}
