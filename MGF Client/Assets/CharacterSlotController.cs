using ExitGames.Client.Photon;
using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotController : MonoBehaviour {

    public GameObject characterSlot;
    public int selectedCharacterId;
    public Button playButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectCharacter()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.instance.SubCodeParameterCode, MessageSubCode.SelectCharacter },
                                                                                                                                                            { (byte)MessageParameterCode.Object, selectedCharacterId } } };
        Debug.Log("Sending Request for Character List");
        PhotonEngine.instance.SendRequest(request);
    }

    public void CharacterSlotSelected(CharacterSlot slot)
    {
        selectedCharacterId = slot.characterId;
        playButton.interactable = true;
    }
}
