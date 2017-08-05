using MultiplayerGameFramework.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Support
{
    public interface IClientConnectionHandler
    {
        void ClientConnect(IClientPeer peer);
        void ClientDisconnect(IClientPeer peer);
    }
}
