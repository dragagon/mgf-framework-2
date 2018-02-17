using ExitGames.Logging;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;

namespace MGF_Photon.Implementation.Handler
{
    public class RegisterSubServerResponseHandler : ServerHandler
    {
        private readonly IServerApplication _serverApplication;
        public ILogger Log { get; set; }

        public RegisterSubServerResponseHandler(ILogger log, IServerApplication serverApplication)
        {
            Log = log;
            _serverApplication = serverApplication;
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
                return 0;
            }
        }

        public override int? SubCode
        {
            get
            {
                return 0;
            }
        }

        public override bool OnHandleMessage(IMessage message, IServerPeer serverPeer)
        {
            if(Log.IsDebugEnabled)
            {
                Log.DebugFormat("Successful Registration");
            }
            _serverApplication.AfterServerRegistration(serverPeer);
            return true;
        }
    }
}
