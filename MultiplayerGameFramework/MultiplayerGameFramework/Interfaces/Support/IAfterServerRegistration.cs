using MultiplayerGameFramework.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Support
{
    public interface IAfterServerRegistration
    {
        void AfterRegister(IServerPeer serverPeer);
    }
}
