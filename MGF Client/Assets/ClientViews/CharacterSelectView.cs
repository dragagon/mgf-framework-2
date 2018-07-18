using ExitGames.Client.Photon;
using GameCommon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.ClientViews
{
    public class CharacterSelectView : MonoBehaviour
    {
        public void Awake()
        {
            RequestCharacterList();
        }

        public void RequestCharacterList()
        {
            OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, MessageSubCode.CharacterList } } };
            Debug.Log("Sending Request for Character List");
            PhotonEngine.instance.SendRequest(request);
        }

        public void CreateNewCharacter()
        {
            SceneManager.LoadScene("CharacterCreate");
        }
    }
}
