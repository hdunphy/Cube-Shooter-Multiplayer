using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : LoadableGameObject
{
    public override void OnLoad()
    {
        RespawnLocation.Instance.AddRespawnLocation(transform.position);
        Destroy(gameObject);
    }
}
