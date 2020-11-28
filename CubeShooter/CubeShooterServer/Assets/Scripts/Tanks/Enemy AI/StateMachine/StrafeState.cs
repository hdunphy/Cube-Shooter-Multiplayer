using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeState : BaseState
{
    public StrafeState(EnemyMovement controller) : base(controller) { }

    public override EnemyAIStateType GetStateType() => EnemyAIStateType.Strafe;

    public override EnemyAIStateType Tick()
    {
        EnemyAIStateType stateType = GetStateType();

        if(controller.TargetedPlayer != null)
        {
            Vector3 targetPosition = controller.TargetedPlayer.transform.position;
            Vector3 offsetPlayer = transform.position - targetPosition;
            Vector3 dir = Vector3.Cross(offsetPlayer, Vector3.up);
            controller.TargetDestination = transform.position + dir;
            //var lookPos = targetPosition - transform.position;
            //lookPos.y = 0;
            //var rotation = Quaternion.
            if (controller.NearestBullet != null)
                stateType = EnemyAIStateType.AvoidBullet;
            else if (Mathf.Abs(offsetPlayer.magnitude) > controller.ChaseDistance)
                stateType = EnemyAIStateType.Search;
        }
        return stateType;
    }
}
