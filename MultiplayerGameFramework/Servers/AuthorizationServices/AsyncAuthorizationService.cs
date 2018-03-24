using Servers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MGF.Domain;
using MGF.Mappers;

namespace Servers.AuthorizationServices
{
    public class AsyncAuthorizationService : IAuthorizationService
    {
        public ReturnCode IsAuthorized(out User user, params string[] authorizationParameters)
        {
            ReturnCode returnCode = ReturnCode.OperationInvalid;

            user = null;
            if (authorizationParameters.Length != 2)
            {
                return ReturnCode.OperationInvalid;
            }

            user = UserMapper.LoadByUserName(authorizationParameters[0]);
            if (null == user)
            {
                return ReturnCode.InvalidUserPass;
            }
            var isAuthorized = AsyncIsAuthorized(authorizationParameters[0], authorizationParameters[0]).Result;

            if(isAuthorized)
            {
                returnCode = ReturnCode.OK;
            }
            else
            {
                returnCode = ReturnCode.InvalidUserPass;
            }

            return returnCode;
        }

        private async Task<bool> AsyncIsAuthorized(string username, string password)
        {
            // Create a task to actually run in a separate thread in order to do OAuth authorization
            var isAuthorized = await Task.Run(() => OAuthIsAuthorized(username, password));
            return isAuthorized;
        }

        private bool OAuthIsAuthorized(string username, string password)
        {
            // Run OAuth specific Code
            // Runs in a separate thread in the thread pool
            return false;
        }
    }
}
