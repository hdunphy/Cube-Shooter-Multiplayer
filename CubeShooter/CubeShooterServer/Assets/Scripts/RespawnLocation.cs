using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    [SerializeField] private List<Vector3> Locations;
    [SerializeField] private float DropHeight;
    [SerializeField] private GameObject SpawnPrefab;

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
    
    private void OnDrawGizmos()
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

    public void LoadRespawnLocations(Transform levelSetUp)
    {
        int childCount = levelSetUp.childCount;
        //Debug.Log($"Level Child Count: {childCount}");
        List<GameObject> objectsToDestroy = new List<GameObject>();

        for(int i = 0; i < childCount; i++)
        {
            var child = levelSetUp.GetChild(i);
            if(child.CompareTag(SpawnPrefab.tag))
            {
                //Debug.Log($"Found spawner at: {child.position}");
                Locations.Add(child.position);
                objectsToDestroy.Add(child.gameObject);
            }
        }

        foreach(GameObject go in objectsToDestroy)
        {
            Destroy(go);
        }
    }
}
