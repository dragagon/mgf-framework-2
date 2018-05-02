using GameCommon;
using MGF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Interfaces
{
    public interface IAuthorizationService
    {
        ReturnCode IsAuthorized(out User user, params string[] authorizationParameters);
        ReturnCode CreateAccount(params string[] authorizationParameters);
    }
}
