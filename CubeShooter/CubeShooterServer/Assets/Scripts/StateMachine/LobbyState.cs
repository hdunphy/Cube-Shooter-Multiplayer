public class LobbyState : IGameState
{
    public void Disconnect(Player player)
    {

    }

    public StateType UpdateState()
    {
        foreach (Client _client in Server.GetAllActiveClients())
            _client.SendIntoGame();
        return StateType.ArenaBattle;
    }
}
