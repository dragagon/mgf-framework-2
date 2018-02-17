using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Modules
{
    public class LoginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            // Normal builder.RegisterType calls
        }

    }
}
