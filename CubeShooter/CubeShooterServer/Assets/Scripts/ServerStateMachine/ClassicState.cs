using System.Linq;

public class ClassicState : IGameState
{
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
        player.SetTankActive(false);

        if (Server.GetAllActiveClients().Count(s => !s.player.GetIsDead()) == 0)
        { //Check if all players are dead
            NetworkManager.Instance.LoadLevel();
        }
        //if all players dead
            //LevelSetup.LoadLevel(current level)
                //set all players active
    }

    public StateType UpdateState()
    {
        return StateType.Lobby;
    }
}
