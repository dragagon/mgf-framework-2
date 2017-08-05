using System.Net;

namespace MultiplayerGameFramework.Implementation.Config
{
    public class PeerInfo
    {
        public IPEndPoint MasterEndPoint { get; set; }
        public int ConnectRetryIntervalSeconds { get; set; }

        public bool IsSiblingConnection { get; set; }
        public int MaxTries { get; set; }
        public int NumTries { get; set; }
        public string ApplicationName { get; set; }

        public PeerInfo(string ipAddress, int ipPort, int connectRetryIntervalSeconds, bool isSiblingConnection, int maxTries, string applicationName)
        {
            MasterEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
            ConnectRetryIntervalSeconds = connectRetryIntervalSeconds;
            IsSiblingConnection = isSiblingConnection;
            MaxTries = maxTries;
            NumTries = 0;
            ApplicationName = applicationName;
        }
    }
}
