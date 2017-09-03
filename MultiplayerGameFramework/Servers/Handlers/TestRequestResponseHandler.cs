using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Implementation.Messaging;

namespace Servers.Handlers
{
    public class TestRequestResponseHandler : IHandler<IClientPeer>
    {
        public MessageType Type => MessageType.Request;

        public byte Code => 1;

        public int? SubCode => 1;

        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            Response response = new Response(Code, SubCode, new Dictionary<byte, object>(), "Response from Request for Response", 0);
            peer.SendMessage(response);
            return true;
        }
    }
}
