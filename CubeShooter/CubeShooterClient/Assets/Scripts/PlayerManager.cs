using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public Transform headTransform;

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
}
