using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreateView : MonoBehaviour {

    public InputField CharacterName;
    public Text CharacterClass;
    // Other information - class, race, stats, etc

    public void ReturnToSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void SendCreateCharacterMessage()
    {
        PhotonEngine.instance.SendRequest(MessageOperationCode.Login, MessageSubCode.CreateCharacter, MessageParameterCode.CharacterName, CharacterName.text, MessageParameterCode.CharacterClass, CharacterClass.text);
    }

    public void SelectCharacterClass(string className)
    {
        CharacterClass.text = className;
    }
}
