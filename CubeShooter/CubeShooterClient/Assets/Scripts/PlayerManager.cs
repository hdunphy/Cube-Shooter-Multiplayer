using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public Color color;
    public Transform headTransform;
    public GameObject tankModel;
    public TextMeshProUGUI usernameText;

    public void SetTankActive(bool _isActive)
    {
        tankModel.SetActive(_isActive);
    }

    public void SpawnPlayer(int _id, string _username)
    {
        id = _id;
        username = _username;

        usernameText.text = _username;
    }
}
