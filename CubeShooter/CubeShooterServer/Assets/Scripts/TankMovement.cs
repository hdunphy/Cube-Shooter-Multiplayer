using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement
{
    private readonly Rigidbody rb;
    private float MaximumVelcoity;
    private float MovementForce;

    public TankMovement(Rigidbody _rb, float _maximumVelocity)
    {
        rb = _rb;
        MaximumVelcoity = _maximumVelocity;
    }

    //public void SetMovement(Vector2 _inputDirection)
    //{
    //    float speedModifier = MovementForce * Time.fixedDeltaTime;
    //    //Vector3 force = new Vector3((horizontalMovement) * speedModifier, 0, Math.Sign(verticleMovement) * speedModifier);
    //    rb.AddForce(force);
    //    if (rb.velocity.magnitude > MaximumVelcoity)
    //    {
    //        rb.velocity = rb.velocity.normalized * MaximumVelcoity;
    //    }
    //}
}
