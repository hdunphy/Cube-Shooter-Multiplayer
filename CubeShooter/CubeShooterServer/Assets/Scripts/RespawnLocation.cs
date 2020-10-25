using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    [SerializeField] private List<Vector3> Locations;
    [SerializeField] private float DropHeight;

    public static RespawnLocation Instance;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        foreach(Vector3 spawnLocation in Locations)
        {
            Gizmos.DrawSphere(spawnLocation, .5f);
        }
    }

    public Vector3 GetRespawnLocation()
    {
        int randIndex = Random.Range(0, Locations.Count);
        return Locations[randIndex] + new Vector3(0, DropHeight, 0);
    }
}
