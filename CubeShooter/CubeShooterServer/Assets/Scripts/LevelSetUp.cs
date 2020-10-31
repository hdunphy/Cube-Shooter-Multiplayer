using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetUp : MonoBehaviour
{
    public List<Vector3> GetWallPositions()
    {
        List<Vector3> wallPositions = new List<Vector3>();
        int wallCount = transform.childCount;

        for(int i = 0; i < wallCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(child.CompareTag("Wall"))
                wallPositions.Add(child.position);
        }

        return wallPositions;
    }
}
