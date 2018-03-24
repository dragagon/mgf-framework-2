using Servers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MGF.Mappers;
using System.Security.Cryptography;
using MGF.Domain;

namespace Servers.AuthorizationServices
{
    public class UserPassAuthorizationService : IAuthorizationService
    {
        public ReturnCode IsAuthorized(out User user, params string[] authorizationParameters)
        {
            user = null;
            if(authorizationParameters.Length != 2)
            {
                return ReturnCode.OperationInvalid;
            }

            user = UserMapper.LoadByUserName(authorizationParameters[0]);
            if(null == user)
            {
                return ReturnCode.InvalidUserPass;
            }
            // valid user, check password
            // create a hash object with SHA2-512
            var sha512 = SHA512Managed.Create();
            // calculate a hash and check it against the password hash in the database
            var hashedpw = sha512.ComputeHash(Encoding.UTF8.GetBytes(authorizationParameters[1]));

            if(user.PasswordHash.Equals(Convert.ToBase64String(hashedpw), StringComparison.OrdinalIgnoreCase))
            {
                return ReturnCode.OK;
            }
            else
            {
                return ReturnCode.InvalidUserPass;
            }
        }
    }
}
