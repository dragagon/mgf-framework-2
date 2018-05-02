using ExitGames.Client.Photon;
using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestView : MonoBehaviour {

    public InputField CreateUserName;
    public InputField CreatePassword;
    public InputField LoginUserName;
    public InputField LoginPassword;

    public void SendResponseRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = 1, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, 1 } } };
        Debug.Log("Sending Request for Response");
        PhotonEngine.instance.SendRequest(request);
    }

    public void SendEventRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, MessageSubCode.LoginUserPass } } };
        Debug.Log("Sending Request for Event");
        PhotonEngine.instance.SendRequest(request);
    }

    public void SendLoginRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, MessageSubCode.LoginUserPass } } };
        request.Parameters.Add((byte)MessageParameterCode.LoginName, LoginUserName.text);
        request.Parameters.Add((byte)MessageParameterCode.Password, LoginPassword.text);
        Debug.Log("Sending Request for Login");
        PhotonEngine.instance.SendRequest(request);

    }

    public void SendNewAccountRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, MessageSubCode.LoginNewAccount } } };
        request.Parameters.Add((byte)MessageParameterCode.LoginName, CreateUserName.text);
        request.Parameters.Add((byte)MessageParameterCode.Password, CreatePassword.text);
        Debug.Log("Sending Request for New Account");
        PhotonEngine.instance.SendRequest(request);

    }
}
