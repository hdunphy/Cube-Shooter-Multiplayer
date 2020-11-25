using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSetUp : MonoBehaviour
{
    [SerializeField] private string Path;
    [SerializeField] private Texture2D levelBitmap;
    [SerializeField] private List<PixelPrefabReference> pixelPrefabReference;

    public List<Vector3> GetWallPositions()
    {
        List<Vector3> wallPositions = new List<Vector3>();
        int wallCount = transform.childCount;

        for (int i = 0; i < wallCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("Wall"))
                wallPositions.Add(child.position);
        }

        return wallPositions;
    }

    private void Start()
    {
        if (levelBitmap == null)
            levelBitmap = Resources.Load(Path) as Texture2D;
        LoadLevelFromPNG();
    }

    public void LoadLevelFromPNG()
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

        RespawnLocation.Instance.LoadRespawnLocations(transform);
    }

    private void GetPrefabFromPixel(int x, int y)
    {
        Color color = levelBitmap.GetPixel(x, y);

        PixelPrefabReference prefabRef = pixelPrefabReference.Find(f => f.color.CompareRGB(color));
        if (prefabRef == null)
            return; //Not in the list

        GameObject go = prefabRef.prefab;
        //Debug.Log($"{color} returns this prefab: {go.name}");
        Instantiate(go, new Vector3(x, 0.5f, y), Quaternion.identity, transform);
    }

    [Serializable]
    private class PixelPrefabReference
    {
        public Color color;
        public GameObject prefab;
    }
}
