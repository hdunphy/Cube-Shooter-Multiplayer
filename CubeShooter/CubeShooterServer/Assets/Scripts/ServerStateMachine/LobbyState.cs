public class LobbyState : IGameState
{
    public void Disconnect(Player player)
    {
        //Should not need anything
    }

    public void EnemyDeath(Enemy enemy)
    {
        //Won't enter
    }

    public void PlayerDeath(Player player)
    {
        //Shouldn't get to here
        throw new System.NotImplementedException();
    }

    public StateType UpdateState()
    {
        NetworkManager.Instance.LoadLevel();
        return StateType.Classic;
    }
}
