using Photon.SocketServer.ServerToServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MGF_Photon.Implementation.Server;

namespace MGF_Photon.Implementation.Server
{
    public class InboundPhotonPeer : InboundS2SPeer
    {
        PhotonServerPeer _serverPeer;
        public PhotonServerPeer ServerPeer
        {
            get { return _serverPeer; }
            set { _serverPeer = value; }
        }
        public InboundPhotonPeer(InitRequest initRequest)
            : base(initRequest)
        {
            SetPrivateCustomTypeCache(new TypeCache().GetCache());
        }


        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            _serverPeer.OnDisconnect(reasonCode, reasonDetail);
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            _serverPeer.OnEvent(eventData, sendParameters);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            _serverPeer.OnOperationRequest(operationRequest, sendParameters);
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            _serverPeer.OnOperationResponse(operationResponse, sendParameters);
        }
    }
}
