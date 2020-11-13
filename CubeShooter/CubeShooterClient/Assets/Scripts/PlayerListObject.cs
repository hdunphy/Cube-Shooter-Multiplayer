using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListObject : MonoBehaviour
{
    public Image ColorIcon;
    public TextMeshProUGUI UserNameText;
    public Image IsReadyIcon;

    
    public void SetPlayerObject(PlayerObject _playerObject)
    {
        ColorIcon.color = _playerObject.Color;
        UserNameText.text = _playerObject.UserName;
    }
}
