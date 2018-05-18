using MultiplayerGameFramework.Interfaces.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Messaging;
using GameCommon;

namespace Servers.Support
{
    public class ClientCodeRemover : IClientCodeRemover
    {
        public void RemoveCodes(IMessage message)
        {
            // Make sure they aren't spoofing the Peer Id
            message.Parameters.Remove((byte)MessageParameterCode.PeerId);
            // Make sure they aren't spoofing the user id
            message.Parameters.Remove((byte)MessageParameterCode.UserId);
        }
    }
}
