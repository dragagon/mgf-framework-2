using Autofac;
using Autofac.Configuration;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Interfaces;
using MultiplayerGameFramework.Interfaces.Support;
using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using System.IO;
using LogManager = ExitGames.Logging.LogManager;
using System;
using Microsoft.Extensions.Configuration;

namespace MGF_Photon
{
    public class PhotonApplication : ApplicationBase
    {
        public ILogger Log { get; set; }
        public IServerApplication ServerApplication { get; set; }
        public IPeerFactory PeerFactory { get; set; }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            var peerConfig = new PeerConfig();
            peerConfig.AddConfig(initRequest);
            return PeerFactory.CreatePeer<PeerBase>(peerConfig);
        }

        protected override ServerPeerBase CreateServerPeer(InitResponse initResponse, object state)
        {
            var peerInfo = state as PeerInfo;
            var peerConfig = new PeerConfig();
            peerConfig.AddConfig(initResponse);
            peerConfig.AddConfig(peerInfo);
            return PeerFactory.CreatePeer<ServerPeerBase>(peerConfig);
        }

        protected override void Setup()
        {
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            Log = LogManager.GetCurrentClassLogger();
            GlobalContext.Properties["LogFileName"] = ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(BinaryPath, "log4net.config")));

            var config = new ConfigurationBuilder();
            config.AddXmlFile(Path.Combine(BinaryPath, ApplicationName + ".config"));
            // config.AddJsonFile(Path.Combine(BinaryPath, ApplicationName + ".json"));

            var module = new ConfigurationModule(config.Build());

            // Create the autofac container
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this).SingleInstance();
            builder.RegisterInstance(Log).As<ILogger>().SingleInstance();
            builder.RegisterModule(module);

            var container = builder.Build();

            ServerApplication = container.Resolve<IServerApplication>();
            ServerApplication.Setup();
            PeerFactory = container.Resolve<IPeerFactory>();
        }

        protected override void TearDown()
        {
            ServerApplication.TearDown();
        }

        protected override void OnStopRequested()
        {
            ServerApplication.TearDown();
            base.OnStopRequested();
        }

        protected override void OnServerConnectionFailed(int errorCode, string errorMessage, object state)
        {
            ServerApplication.OnServerConnectionFailed(errorCode, errorMessage, state);
        }
    }
}
