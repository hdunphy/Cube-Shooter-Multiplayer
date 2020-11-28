using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyAIStateType { Search, AvoidBullet, Strafe, None }

public class EnemyAIStateMachine
{

    private Dictionary<EnemyAIStateType, BaseState> _availableStates;
    public BaseState CurrentState { get; private set; }
    public event Action<BaseState> OnStateChange;

    public EnemyAIStateMachine(Dictionary<EnemyAIStateType, BaseState> states)
    {
        _availableStates = states;
    }

    // Update is called once per frame
    public void Update()
    {
        if(CurrentState == null)
        {
            CurrentState = _availableStates.Values.First();
        }

        EnemyAIStateType? nextState = CurrentState?.Tick();

        if(nextState != null && nextState != CurrentState?.GetStateType())
        {
            SwitchOnNextState(nextState.Value);
        }
    }

    private void SwitchOnNextState(EnemyAIStateType nextState)
    {
        CurrentState = _availableStates[nextState];
        OnStateChange?.Invoke(CurrentState);
    }
}
