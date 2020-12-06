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
        { //Check if all players are dead. (number of players not dead == 0)
            /*NetworkManager.Instance.LoadLevel(); */

            //Need to reset the level
            NetworkManager.Instance.ResetLevel();

            //SendServer to return to lobby
            ServerSend.EndLevel(false);
            //NetworkManager.Instance.NextState();
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
