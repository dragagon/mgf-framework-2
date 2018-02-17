using System;

namespace MGF_Photon.Implementation.Operation.Data
{
    public class RegisterSubServerData
    {
        public RegisterSubServerData()
        {
        }

        public string GameServerAddress { get; set; }
        public Guid? ServerId { get; set; }
        public int? TcpPort { get; set; }
        public int? UdpPort { get; set; }
        public int ServerType { get; set; }
        public string ServerName { get; set; }
    }
}