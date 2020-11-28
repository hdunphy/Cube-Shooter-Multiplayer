using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{

    public SearchState(EnemyMovement controller) : base(controller) { }
    public override EnemyAIStateType GetStateType() => EnemyAIStateType.Search;

    public override EnemyAIStateType Tick()
    {
        EnemyAIStateType state = GetStateType();

        if(controller.TargetedPlayer != null)
        {
            Vector3 targetPosition = controller.TargetedPlayer.transform.position;
            //Vector3 offsetPlayer = transform.position - targetPosition;
            float distance = Vector3.Distance(transform.position, targetPosition);
            controller.TargetDestination = targetPosition;

            if (controller.NearestBullet != null)
                state = EnemyAIStateType.AvoidBullet;
            else if (Mathf.Abs(distance) < controller.StrafeDistance)
                state = EnemyAIStateType.Strafe;
        }

        return state;
    }
}
