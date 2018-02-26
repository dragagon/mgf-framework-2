using ExitGames.Client.Photon;
using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : MonoBehaviour {

    public void SendResponseRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = 1, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, 1 } } };
        Debug.Log("Sending Request for Response");
        PhotonEngine.instance.SendRequest(request);
    }

    public void SendEventRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, 2 } } };
        Debug.Log("Sending Request for Event");
        PhotonEngine.instance.SendRequest(request);
    }
}
