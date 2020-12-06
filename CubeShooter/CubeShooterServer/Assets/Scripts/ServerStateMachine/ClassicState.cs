using System.Linq;

public class ClassicState : IGameState
{
    private int currentLevelIndex;

    public ClassicState()
    {
        currentLevelIndex = 0;
        NetworkManager.Instance.SetLevelIndex(currentLevelIndex);
    }

    public void Disconnect(Player player)
    {
        BulletObjectPool.Instance.RemoveBulletsFromPlayer(player);
        //Remove bullets connected to player
        //player isn't always being removed
        UnityEngine.Object.Destroy(player.gameObject);

        if (!Server.GetAllActiveClients().Any())
            NetworkManager.Instance.NextState();
    }

    public void EnemyDeath(Enemy enemy)
    {
        enemy.Destroy();

        if(NetworkManager.Instance.GetEnemies().Length == 1)
        {
            NetworkManager.Instance.ResetLevel(true);
            bool hasLevelLeft = NetworkManager.Instance.SetLevelIndex(++currentLevelIndex);
                
        }
    }

    public void PlayerDeath(Player player)
    {
        player.SetTankActive(false);

        if (Server.GetAllActiveClients().Count(s => !s.player.GetIsDead()) == 0)
        { //Check if all players are dead. (number of players not dead == 0)
            //Need to reset the level
            NetworkManager.Instance.ResetLevel(false);
        }
    }

    public StateType UpdateState()
    {
        foreach (Client _client in Server.GetAllActiveClients())
            _client.isReady = false;
        ServerSend.ConnectToLobby();
        return StateType.Lobby;
    }
}
