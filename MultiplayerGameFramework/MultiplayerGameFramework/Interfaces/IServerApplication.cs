using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Interfaces.Server;

namespace MultiplayerGameFramework.Interfaces
{
    public interface IServerApplication
    {
        byte SubCodeParameterCode { get; }
        string BinaryPath { get; }
        string ApplicationName { get; }

        void Setup();
        void TearDown();
        void OnServerConnectionFailed(int errorCode, string errorMessage, object state);
        void AfterServerRegistration(IServerPeer peer);
        void ConnectToPeer(PeerInfo peerInfo);
    }
}
