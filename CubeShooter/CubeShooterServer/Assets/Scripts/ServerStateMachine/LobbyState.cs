public class LobbyState : IGameState
{
    public void Disconnect(Player player)
    {
        //Should not need anything
    }

    public void PlayerDeath(Player player)
    {
        //Shouldn't get to here
        throw new System.NotImplementedException();
    }

    public StateType UpdateState()
    {
        foreach (Client _client in Server.GetAllActiveClients())
            _client.SendIntoGame();
        return StateType.ArenaBattle;
    }
}
