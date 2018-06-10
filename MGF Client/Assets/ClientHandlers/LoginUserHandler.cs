using GameCommon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.ClientHandlers
{
    public class LoginUserHandler : GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if(returnCode == (short)ReturnCode.OK)
            {
                // Successful Login
                SceneManager.LoadScene("CharacterSelect");
            }
            else
            {
                Debug.LogFormat("{0} - {1}", this.name, debugMessage);
            }
        }
    }
}
