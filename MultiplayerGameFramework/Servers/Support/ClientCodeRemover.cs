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
            // Do not remove any codes yet, nothing to remove.
            message.Parameters.Remove((byte)MessageParameterCode.PeerId);
        }
    }
}
