using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected EnemyMovement controller;
    protected Transform transform;

    public BaseState(EnemyMovement controller)
    {
        this.controller = controller;
        transform = controller.transform;
    }
    public abstract EnemyAIStateType Tick();

    public abstract EnemyAIStateType GetStateType();
}
