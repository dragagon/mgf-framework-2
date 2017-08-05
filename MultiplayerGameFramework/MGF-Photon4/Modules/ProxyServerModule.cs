using Autofac;
using MGF_Photon.Implementation;
using MGF_Photon.Implementation.Client;
using MGF_Photon.Implementation.Data;
using MGF_Photon.Implementation.Handler;
using MGF_Photon.Implementation.Server;
using MultiplayerGameFramework.Implementation.Client;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Implementation.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF_Photon.Modules
{
    public class ProxyServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ServerApplication>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PhotonPeerFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PeerConfig>().AsImplementedInterfaces();
            builder.RegisterType<SubServerClientPeer>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<PhotonClientPeer>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<PhotonServerPeer>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<ServerConnectionCollection>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ClientConnectionCollection>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ServerHandlerList>().AsImplementedInterfaces();
            builder.RegisterType<ClientHandlerList>().AsImplementedInterfaces();

            // Forwarding events and responses back to the client - ingores requests (outputs error)
            builder.RegisterType<EventForwardHandler>().AsImplementedInterfaces();
            builder.RegisterType<RequestForwardHandler>().AsImplementedInterfaces();
            builder.RegisterType<ResponseForwardHandler>().AsImplementedInterfaces();

            builder.RegisterType<HandleServerRegistration>().AsImplementedInterfaces();
            builder.RegisterType<ServerData>().AsImplementedInterfaces();
        }
    }
}
