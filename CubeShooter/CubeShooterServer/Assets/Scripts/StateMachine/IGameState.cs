public interface IGameState
{
    void Disconnect(Player player);
    StateType UpdateState();
}
