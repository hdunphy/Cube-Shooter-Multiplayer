using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBattleState : IGameState
{
    public void Disconnect(Player player)
    {
        BulletObjectPool.Instance.RemoveBulletsFromPlayer(player);
        //Remove bullets connected to player
        //player isn't always being removed
        UnityEngine.Object.Destroy(player.gameObject);
    }
}
