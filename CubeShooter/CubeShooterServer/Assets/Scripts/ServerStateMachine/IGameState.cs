public interface IGameState
{
    void PlayerDeath(Player player);
    void EnemyDeath(Enemy enemy);
    void Disconnect(Player player);
    StateType UpdateState();
}
