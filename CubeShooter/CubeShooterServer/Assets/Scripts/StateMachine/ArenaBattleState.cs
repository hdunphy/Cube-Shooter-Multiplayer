public class ArenaBattleState : IGameState
{
    public void Disconnect(Player player)
    {
        BulletObjectPool.Instance.RemoveBulletsFromPlayer(player);
        //Remove bullets connected to player
        //player isn't always being removed
        UnityEngine.Object.Destroy(player.gameObject);
    }

    public StateType UpdateState()
    {
        return StateType.Lobby;
    }
}
