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
    public TextMeshPro usernameText;

    public void Respawn(float seconds)
    {
        StartCoroutine(RespawnCoroutine(seconds));
    }

    private IEnumerator RespawnCoroutine(float seconds)
    {
        transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(seconds);

        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SpawnPlayer(int _id, string _username)
    {
        id = _id;
        username = _username;

        usernameText.text = _username;
    }
}
