using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Implementation.Messaging;
using GameCommon;
using ExitGames.Logging;
using Servers.Interfaces;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Support;
using MultiplayerGameFramework.Implementation.Config;
using Servers.Data.Client;
using MGF.Domain;

namespace Servers.Handlers.Login
{
    public class LoginAuthenticationHandler : IHandler<IServerPeer>
    {
        private ILogger Log;
        private IAuthorizationService AuthService;
        private IConnectionCollection<IClientPeer> ConnectionCollection;
        private IPeerFactory PeerFactory;
        public LoginAuthenticationHandler(ILogger log, IAuthorizationService authService, IConnectionCollection<IClientPeer> connectionCollection, IPeerFactory peerFactory)
        {
            Log = log;
            AuthService = authService;
            ConnectionCollection = connectionCollection;
            PeerFactory = peerFactory;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int)MessageSubCode.LoginUserPass;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            Response response;
            // If not enough arguments, ok to return Invalid - Normally we return InvalidUserPass
            if (!message.Parameters.ContainsKey((byte)MessageParameterCode.LoginName) || !message.Parameters.ContainsKey((byte)MessageParameterCode.Password))
            {
                Log.DebugFormat("Sending Invalid Operation Response");
                response = new Response(Code, SubCode, new Dictionary<byte, object>() { {(byte)MessageParameterCode.SubCodeParameterCode, SubCode }, {(byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Not enough arguments", (short)ReturnCode.OperationInvalid);
                peer.SendMessage(response);
            }
            else
            {
                // Use our preferred Authorization Service to check if authorized
                User user = null;
                var returnCode = AuthService.IsAuthorized(out user, (string)message.Parameters[(byte)MessageParameterCode.LoginName], (string)message.Parameters[(byte)MessageParameterCode.Password]);


                if (returnCode == ReturnCode.OK)
                {
                    // Need to add this login to the list of clients connected complete with the user id for other login purposes.
                    var clientpeer = PeerFactory.CreatePeer<IClientPeer>(new PeerConfig());
                    clientpeer.PeerId = new Guid((byte[])message.Parameters[(byte)MessageParameterCode.PeerId]);

                    // Add to connection collection
                    ConnectionCollection.Connect(clientpeer);

                    // Add our user id to the client peer for when we do character selection/etc
                    clientpeer.ClientData<CharacterData>().UserId = user.Id;

                    // We know it's OK. send the user ID back to proxy server, to send to the client.
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] }, { (byte)MessageParameterCode.UserId, user.Id } }, "", (short)returnCode);
                    peer.SendMessage(response);
                }
                else
                {
                    // UserPass is not good.
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Invalid Username or Password", (short)returnCode);
                    peer.SendMessage(response);
                }
            }
            return true;
        }
    }
}
