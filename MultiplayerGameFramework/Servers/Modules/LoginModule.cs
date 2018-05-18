using Autofac;
using Servers.AuthorizationServices;
using Servers.Data.Client;
using Servers.Handlers.Login;
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
            builder.RegisterType<LoginAuthenticationHandler>().AsImplementedInterfaces();
            builder.RegisterType<LoginAccountCreationHandler>().AsImplementedInterfaces();
            builder.RegisterType<CharacterData>().AsImplementedInterfaces();
            builder.RegisterType<UserSaltedPassAuthorizationService>().AsImplementedInterfaces();
            builder.RegisterType<LoginCharacterListCharactersHandler>().AsImplementedInterfaces();
        }

    }
}
