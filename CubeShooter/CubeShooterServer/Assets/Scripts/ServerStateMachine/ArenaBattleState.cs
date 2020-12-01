using System.Linq;

public class ArenaBattleState : IGameState
{
    public float RespawnTime = 3f;

    public void Disconnect(Player player)
    {
        BulletObjectPool.Instance.RemoveBulletsFromPlayer(player);
        //Remove bullets connected to player
        //player isn't always being removed
        UnityEngine.Object.Destroy(player.gameObject);

        if (!Server.GetAllActiveClients().Any())
            NetworkManager.Instance.NextState();
    }

    public void PlayerDeath(Player player)
    {
        player.Respawn(RespawnTime);
    }

    public StateType UpdateState()
    {
        //Reset level ?
        return StateType.Lobby;
    }
}
