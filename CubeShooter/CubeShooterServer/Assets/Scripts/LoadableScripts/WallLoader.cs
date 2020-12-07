using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallLoader", menuName = "ScriptableObjects/Tanks/WallLoader")]
public class WallLoader : LoadableGameObject
{
    public GameObject wallPrefab;
    [SerializeField] private Color color;
    public override Color GetBaseColor()
    {
        return color;
    }

    public override void OnLoad(Vector3 _position, Transform parent)
    {
        Instantiate(wallPrefab, _position, Quaternion.identity, parent);
    }
}
