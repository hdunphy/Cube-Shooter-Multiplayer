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

    [SerializeField] private Sprite CheckedIcon;
    [SerializeField] private Sprite UncheckedIcon;


    public void SetPlayerObject(PlayerObject _playerObject)
    {
        ColorIcon.color = _playerObject.Color;
        UserNameText.text = _playerObject.UserName;
        IsReadyIcon.sprite = _playerObject.IsReady ? CheckedIcon : UncheckedIcon;
    }
}
