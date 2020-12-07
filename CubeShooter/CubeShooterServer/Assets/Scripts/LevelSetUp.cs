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
    [SerializeField] private List<Texture2D> classicModeLevels;
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

        //if (!color.Compare(Color.white) && !color.Compare(Color.black))
        //    Debug.Log(color);

        LoadableGameObject prefabRef = loadableGameObjects.Find(f => CompareColors(color, f.GetBaseColor()));
        if (prefabRef == null)
            return; //Not in the list
        prefabRef.OnLoad(new Vector3(x, 0.5f, y), transform);
    }

    public bool SetLevelIndex(int levelIndex)
    {
        bool hasAnotherLevel = false;
        if (levelIndex < classicModeLevels.Count)
        {
            hasAnotherLevel = true;
            levelBitmap = classicModeLevels[levelIndex];
            Debug.Log($"Set level to: {levelBitmap.name}");
        }

        return hasAnotherLevel;
    }

    public void ResetLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }

        RespawnLocation.Instance.ResetRespawnLocations();
    }

    private bool CompareColors(Color colorA, Color colorB)
    {
        float tolerance = 0.02f;

        bool r = Mathf.Abs(colorA.r - colorB.r) <= tolerance;
        bool g = Mathf.Abs(colorA.g - colorB.g) <= tolerance;
        bool a = Mathf.Abs(colorA.b - colorB.b) <= tolerance;
        bool b = Mathf.Abs(colorA.a - colorB.a) <= tolerance;

        return r && g && a && b;
    }

    private Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        return croppedTexture;
    }
}
