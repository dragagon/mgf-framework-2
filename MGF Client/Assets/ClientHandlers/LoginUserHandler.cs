using GameCommon;
using System;
using UnityEngine.SceneManagement;

namespace Assets.ClientHandlers
{
    public class LoginUserHandler : IMessageHandler
    {
        public MessageType Type
        {
            get
            {
                return MessageType.Response;
            }
        }

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.LoginUserPass;

        public bool HandleMessage(IMessage message)
        {
            var response = message as Response;
            if(response.ReturnCode == (short)ReturnCode.OK)
            {
                // Successful Login
                SceneManager.LoadScene("CharacterSelect");
            }
            else
            {
                // Unsuccessful
                // ShowError(response.DebugMessage);
            }
            return true;
        }
    }
}
