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
    public Transform headTransform;
    public GameObject tankModel;
    public TextMeshProUGUI usernameText;

    public void Respawn(float seconds)
    {
        StartCoroutine(RespawnCoroutine(seconds));
    }

    private IEnumerator RespawnCoroutine(float seconds)
    {
        tankModel.SetActive(false);

        yield return new WaitForSeconds(seconds);

        tankModel.SetActive(true);
    }

    public void SpawnPlayer(int _id, string _username)
    {
        id = _id;
        username = _username;

        usernameText.text = _username;
    }
}
