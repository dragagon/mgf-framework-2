using GameCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ClientHandlers
{
    public class ListCharacterHandler : IMessageHandler
    {
        public MessageType Type
        {
            get
            {
                return MessageType.Response;
            }
        }

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.CharacterList;

        public bool HandleMessage(IMessage message)
        {
            var response = message as Response;
            if (response.ReturnCode == (short)ReturnCode.OK)
            {
                // Show the login screen since it was successful
                Debug.LogFormat("Character List - {0}", message.Parameters[(byte)MessageParameterCode.Object]);
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
