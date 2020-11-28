using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory
{
    public static BaseState CreateBaseState(EnemyAIStateType stateType, EnemyMovement controller)
    {
        BaseState returnState = null;
        switch (stateType)
        {
            case EnemyAIStateType.AvoidBullet:
                returnState = new AvoidBulletState(controller);
                break;
            case EnemyAIStateType.Search:
                returnState = new SearchState(controller);
                break;
            case EnemyAIStateType.Strafe:
                returnState = new StrafeState(controller);
                break;
        }
        return returnState;
    }
}
