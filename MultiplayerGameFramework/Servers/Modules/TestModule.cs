using Autofac;
using Servers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Modules
{
    public class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            // Normal builder.RegisterType calls
            builder.RegisterType<TestRequestResponseHandler>().AsImplementedInterfaces();
            builder.RegisterType<TestRequestEventHandler>().AsImplementedInterfaces();
        }
    }
}
