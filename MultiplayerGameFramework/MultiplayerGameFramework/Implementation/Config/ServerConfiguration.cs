using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Config
{
    public class ServerConfiguration
    {
        public bool AllowPhysicalClients { get; set; }
        public int ParentPort { get; set; }
        public int SiblingPort { get; set; }
        public byte SubCodeParameterCode { get; set; }
        public byte PeerIdCode { get; set; }

        public string PublicIpAddress { get; set; }
        public int? TcpPort { get; set; }
        public int? UdpPort { get; set; }
        public Guid? ServerId { get; set; }
        public int ServerType { get; set; }
        public string ServerName { get; set; }

        public ServerConfiguration(bool allowPhysicalClients, int parentPort, int siblingPort, byte subCodeParameterCode, byte peerIdCode)
        {
            AllowPhysicalClients = allowPhysicalClients;
            ParentPort = parentPort;
            SiblingPort = siblingPort;
            SubCodeParameterCode = subCodeParameterCode;
            PeerIdCode = peerIdCode;
        }
    }
}
