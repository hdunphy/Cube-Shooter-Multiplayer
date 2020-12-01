public interface IGameState
{
    void PlayerDeath(Player player);
    void Disconnect(Player player);
    StateType UpdateState();
}
