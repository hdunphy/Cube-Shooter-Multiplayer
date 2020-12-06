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

    private void Start()
    {
        setBuildNavMesh = false;
        if (levelBitmap == null)
            levelBitmap = Resources.Load(Path) as Texture2D;
        //LoadLevelFromPNG();
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
    }

    public void LoadLevel()
    {
        //Reset Components
        //ResetLevel();
        //Tell Client to reset

        //Reload Everything
        LoadLevelFromPNG();

        //Respawn all players

        //foreach (Player _player in FindObjectsOfType<Player>())
        //    _player.Respawn();
    }

    public void ResetLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
