using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawner", menuName = "ScriptableObjects/Tanks/PlayerSpawner")]
public class PlayerSpawner : LoadableGameObject
{
    [SerializeField] private Color color;
    public override Color GetBaseColor()
    {
        return color;
    }

    public override void OnLoad(Vector3 _position, Transform _partent)
    {
        RespawnLocation.Instance.AddRespawnLocation(_position);
        //Destroy(gameObject);
    }
}
