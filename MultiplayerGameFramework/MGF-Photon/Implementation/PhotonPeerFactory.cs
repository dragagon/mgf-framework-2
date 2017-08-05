using ExitGames.Logging;
using MGF_Photon.Implementation.Client;
using MGF_Photon.Implementation.Server;
using MultiplayerGameFramework.Implementation.Client;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Support;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF_Photon.Implementation
{
    public class PhotonPeerFactory : IPeerFactory
    {
        public ILogger Log { get; set; }
        public bool AllowPhysicalClients { get; set; }
        public int ParentPort { get; set; }
        public int SiblingPort { get; set; }

        public PhotonServerPeer.ServerPeerFactory ServerPeerFactory { get; set; }
        public PhotonClientPeer.ClientPeerFactory ClientPeerFactory { get; set; }

        public SubServerClientPeer.ClientPeerFactory SubServerClientPeerFactory { get; set; }

        public PhotonPeerFactory(ServerConfiguration serverConfiguration, ILogger log, PhotonServerPeer.ServerPeerFactory serverPeerFactory,
            PhotonClientPeer.ClientPeerFactory clientPeerFactory, SubServerClientPeer.ClientPeerFactory subServerClientPeerFactory)
        {
            AllowPhysicalClients = serverConfiguration.AllowPhysicalClients;
            ParentPort = serverConfiguration.ParentPort;
            SiblingPort = serverConfiguration.SiblingPort;
            Log = log;
            ServerPeerFactory = serverPeerFactory;
            ClientPeerFactory = clientPeerFactory;
            SubServerClientPeerFactory = subServerClientPeerFactory;
            Log.DebugFormat("Port data from Configuration. Parent {0}, Sibling {1}, Allow Clients {2}", ParentPort, SiblingPort, AllowPhysicalClients);
        }

        public T CreatePeer<T>(IPeerConfig config) where T : class
        {
            // Client and server incomming
            var initRequest = config.GetConfig<InitRequest>();
            // we requested a connection to another server
            var initResponse = config.GetConfig<InitResponse>();
            if(initRequest != null)
            {
                Log.DebugFormat("Connection request on port {0}", initRequest.LocalPort);
                if(initRequest.LocalPort == ParentPort)
                {
                    // If not sibling
                    return ServerPeerFactory(initRequest.Protocol, initRequest.PhotonPeer, false) as T;
                }
                else if(initRequest.LocalPort == SiblingPort)
                {
                    // if sibling
                    return ServerPeerFactory(initRequest.Protocol, initRequest.PhotonPeer, true) as T;
                }
                if(AllowPhysicalClients)
                {
                    return ClientPeerFactory(initRequest.Protocol, initRequest.PhotonPeer) as T;
                }
                return null;
            }
            else if(initResponse != null)
            {
                var peerInfo = config.GetConfig<PeerInfo>();
                if(peerInfo != null)
                {
                    var serverPeer = ServerPeerFactory(initResponse.Protocol, initResponse.PhotonPeer, peerInfo.IsSiblingConnection);

                    serverPeer.Register();

                    return serverPeer as T;
                }
                return null;
            }
            else
            {
                return SubServerClientPeerFactory() as T;
            }
        }
    }
}
