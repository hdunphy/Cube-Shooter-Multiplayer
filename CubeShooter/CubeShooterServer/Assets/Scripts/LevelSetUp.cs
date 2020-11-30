using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class LevelSetUp : MonoBehaviour
{
    [SerializeField] private string Path;
    [SerializeField] private Texture2D levelBitmap;
    [SerializeField] private List<LoadableGameObject> loadableGameObjects;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private bool setBuildNavMesh;

    //public List<Vector3> GetWallPositions()
    //{
    //    List<Vector3> wallPositions = new List<Vector3>();
    //    int wallCount = transform.childCount;

    //    for (int i = 0; i < wallCount; i++)
    //    {
    //        Transform child = transform.GetChild(i);
    //        if (child.CompareTag("Wall"))
    //            wallPositions.Add(child.position);
    //    }

    //    return wallPositions;
    //}

    private void Start()
    {
        setBuildNavMesh = false;
        if (levelBitmap == null)
            levelBitmap = Resources.Load(Path) as Texture2D;
        LoadLevelFromPNG();
    }

    public void SetBuildNaveMesh(bool _setBuildNavMesh)
    {
        setBuildNavMesh = _setBuildNavMesh;
    }

    //For later
    public void SetLevel(Texture2D level)
    {
        levelBitmap = level;
        LoadLevelFromPNG();
    }

    private void LoadLevelFromPNG()
    {
        int width = levelBitmap.width;
        int height = levelBitmap.height;
        Debug.Log($"image loaded with h: {height} and w: {width}");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetPrefabFromPixel(x, y);
            }
        }

        if (setBuildNavMesh)
        {
            Debug.Log("Building Nav Mesh");
            navMeshSurface.BuildNavMesh();
            setBuildNavMesh = false;
        }
    }

    private void GetPrefabFromPixel(int x, int y)
    {
        Color color = levelBitmap.GetPixel(x, y);

        LoadableGameObject prefabRef = loadableGameObjects.Find(f => f.GetBaseColor().CompareRGB(color));
        if (prefabRef == null)
            return; //Not in the list
        prefabRef.OnLoad(new Vector3(x, 0.5f, y), transform);

        //LoadableGameObject go = Instantiate(prefabRef, new Vector3(x, 0.5f, y), Quaternion.identity, transform);
        //Debug.Log($"{color} returns this prefab: {go.name}");

        //go.OnLoad();
    }
}
