using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public GameObject playerPrefab;
    [SerializeField] private LevelSetUp levelSetUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(20, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public List<Vector3> GetWallPositions()
    {
        return levelSetUp.GetWallPositions();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, RespawnLocation.Instance.GetRespawnLocation(), Quaternion.identity).GetComponent<Player>();
    }
}
