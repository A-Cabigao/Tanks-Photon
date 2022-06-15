using UnityEngine;
using Photon.Pun;
using TMPro;

public class NameInputField : MonoBehaviour
{
    TMP_InputField field;
    void Awake() {
        field = GetComponent<TMP_InputField>();
    }
    public void SetPlayerName(string nameInput)
    {
        if (string.IsNullOrEmpty(nameInput))
        {
            Debug.LogError("Player name cannot be empty");
            return;
        }
        // limit character count to avoid UI issues
        // 11 refers to the max length allowed in other player UI
        if(nameInput.Length > 11){
            nameInput = nameInput.Substring(0,11);
            field.text = nameInput;
        }
        PhotonNetwork.NickName = nameInput;
    }
}
