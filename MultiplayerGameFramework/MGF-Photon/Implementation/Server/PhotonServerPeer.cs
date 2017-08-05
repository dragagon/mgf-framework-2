using ExitGames.Logging;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;

namespace MGF_Photon.Implementation.Server
{
    public class PhotonServerPeer : ServerPeerBase, IServerPeer
    {
        private readonly Dictionary<Type, IServerData> _serverData = new Dictionary<Type, IServerData>();

        public bool Registered { get; set; }
        public ILogger Log { get; set; }
        private ServerApplication _server;
        public IServerApplication Server
        {
            get { return _server; }
            set { _server = value as ServerApplication; }
        }

        public IServerType ServerType { get; set; }
        private readonly IServerConnectionCollection<IServerType, IServerPeer> _serverCollection;
        private readonly IHandlerList<IServerPeer> _handlerList;

        #region Factory Method

        public delegate PhotonServerPeer ServerPeerFactory(IRpcProtocol protocol, IPhotonPeer photonPeer, bool isPeer);

        #endregion

        public PhotonServerPeer(IRpcProtocol protocol, IPhotonPeer photonPeer, bool isPeer,
            ILogger log,
            IServerApplication server,
            IEnumerable<IServerData> serverData,
            IServerConnectionCollection<IServerType, IServerPeer> serverCollection,
            IHandlerList<IServerPeer> handlerList)
            : base(protocol, photonPeer)
        {
            Log = log;
            Server = server;
            Log.DebugFormat("Created Server Peer");
            _serverCollection = serverCollection;
            _handlerList = handlerList;
            _serverCollection.Connect(this);
            foreach(var data in serverData)
            {
                Log.DebugFormat("data {0}-{1}", data.ToString(), data.GetType().ToString());
                _serverData.Add(data.GetType(), data);
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            HandleMessage(new Request(operationRequest.OperationCode,
                operationRequest.Parameters.ContainsKey(Server.SubCodeParameterCode) ?
                  (int?)(Convert.ToInt32(operationRequest.Parameters[Server.SubCodeParameterCode])) : null,
                operationRequest.Parameters));
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            HandleMessage(new Event(eventData.Code,
                eventData.Parameters.ContainsKey(Server.SubCodeParameterCode) ?
                  (int?)(Convert.ToInt32(eventData.Parameters[Server.SubCodeParameterCode])) : null,
                eventData.Parameters));
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            HandleMessage(new Response(operationResponse.OperationCode,
                operationResponse.Parameters.ContainsKey(Server.SubCodeParameterCode) ?
                  (int?)(Convert.ToInt32(operationResponse.Parameters[Server.SubCodeParameterCode])) : null,
                operationResponse.Parameters,
                operationResponse.DebugMessage,
                operationResponse.ReturnCode));
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            _serverCollection.Disconnect(this);
            Log.DebugFormat("Server Disconnected {0} - {1}", reasonCode.ToString(), reasonDetail);
        }

        public void Register()
        {
            _server.Register(this);
        }

        public T ServerData<T>() where T : class, IServerData
        {
            IServerData result;
            _serverData.TryGetValue(typeof(T), out result);
            if(result != null)
            {
                return result as T;
            }
            return null;
        }

        public void HandleMessage(IMessage message)
        {
            _handlerList.HandleMessage(message, this);
        }

        public void SendMessage(IMessage message)
        {
            var response = message as Response;
            if(response != null)
            {
                SendOperationResponse(new OperationResponse(message.Code, message.Parameters) { DebugMessage = response.DebugMessage, ReturnCode = response.ReturnCode }, new SendParameters());
            }
            else if(message is Event)
            {
                SendEvent(new EventData(message.Code, message.Parameters), new SendParameters());
            }
            else if(message is Request)
            {
                SendOperationRequest(new OperationRequest(message.Code, message.Parameters), new SendParameters());
            }
        }
    }
}
