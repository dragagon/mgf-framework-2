using Autofac;
using Servers.BackgroundThreads;
using Servers.Config;
using Servers.Data.Client;
using Servers.Handlers.Proxy;
using Servers.Support;

namespace Servers.Modules
{
    public class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            // Normal builder.RegisterType calls
            //builder.RegisterType<TestRequestResponseHandler>().AsImplementedInterfaces();
            //builder.RegisterType<TestRequestEventHandler>().AsImplementedInterfaces();
            builder.RegisterType<TestBackgroundThread>().AsImplementedInterfaces();
            builder.RegisterType<ClientCodeRemover>().AsImplementedInterfaces();
            builder.RegisterType<ServerType>().AsImplementedInterfaces();
            builder.RegisterType<CharacterData>().AsImplementedInterfaces();
            builder.RegisterType<ClientLoginForwardingRequestHandler>().AsImplementedInterfaces();
        }
    }
}
