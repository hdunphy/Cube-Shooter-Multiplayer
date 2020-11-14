using System;
using System.Collections.Generic;
using System.Linq;

public enum StateType { Lobby, ArenaBattle }
public class GameStateMachine
{
    private Dictionary<StateType, IGameState> _availableStates;
    public IGameState CurrentState { get; private set; }
    public event Action<IGameState> OnStateChange;

    public GameStateMachine(Dictionary<StateType, IGameState> states)
    {
        _availableStates = states;
        CurrentState = _availableStates.Values.First();
    }
}
