using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetUp : MonoBehaviour
{

    [SerializeField] private List<PixelPrefabReference> pixelPrefabReference;
    private Texture2D levelBitmap;

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

    private void Start()
    {
        LoadLevelFromPNG("LevelBitmaps/Level");
    }

    public void LoadLevelFromPNG(string path)
    {
        levelBitmap = Resources.Load(path) as Texture2D;

        int width = levelBitmap.width;
        int height = levelBitmap.height;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GetPrefabFromPixel(x, y);
            }
        }
    }

    private void GetPrefabFromPixel(int x, int y)
    {
        Color color = levelBitmap.GetPixel(x, y);

        //use for ground
        if (color.Equals(Color.white))
            return;


        GameObject go = pixelPrefabReference.Find(f => f.color == color).prefab;
        Debug.Log($"{color} returns this prefab: {go.name}");
        Instantiate(go, new Vector3(x, 1, y), Quaternion.identity, transform);
    }

    [Serializable]
    private class PixelPrefabReference
    {
        public Color color;
        public GameObject prefab;
    }
}
