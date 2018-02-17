using GameCommon;
using MultiplayerGameFramework.Interfaces.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Config
{
    public class ServerType : IServerType
    {
        public static ServerType ProxyServer = new ServerType() { Name = "Proxy" };
        public static ServerType LoginServer = new ServerType() { Name = "Login" };

        public string Name { get; set; }

        public IServerType GetServerType(int serverType)
        {
            IServerType server = null;

            switch(serverType)
            {
                case 1:
                    server = LoginServer;
                    break;
                case 0:
                default:
                    server = ProxyServer;
                    break;
            }

            return server;
        }
    }
}
