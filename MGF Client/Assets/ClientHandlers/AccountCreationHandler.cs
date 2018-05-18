using GameCommon;
using System;
using UnityEngine;

namespace Assets.ClientHandlers
{
    public class AccountCreationHandler : IMessageHandler
    {
        public MessageType Type
        {
            get
            {
                return MessageType.Response;
            }
        }

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.LoginNewAccount;

        public bool HandleMessage(IMessage message)
        {
            var response = message as Response;
            if(response.ReturnCode == (short)ReturnCode.OK)
            {
                // Show the login screen since it was successful
                Debug.LogFormat("Account Created Successfully");
            }
            else
            {
                // Show the error dialog
                // ShowError(response.DebugMessage);
                Debug.LogFormat("{0}", response.DebugMessage);
            }
            return true;
        }
    }
}
