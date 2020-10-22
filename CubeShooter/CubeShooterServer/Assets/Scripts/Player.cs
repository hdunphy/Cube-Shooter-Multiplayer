using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    [SerializeField] private TankData tankMovementData;
    [SerializeField] private Rigidbody rb;
    public Transform headTransform;
    private bool isDead;

    private float forceModifier;
    private bool[] inputs;
    private Vector3 MousePosition;

    /*Firing*/
    private bool isShooting;
    private float nextFire;
    private int bulletCount;

    private float FireRate => tankMovementData.FireRate;
    private float BulletDistanceOffset => tankMovementData.BulletDistanceOffset;
    private float BulletVelocity => tankMovementData.BulletVelocity;
    private int NumberOfBulletBounces => tankMovementData.NumberOfBulletBounces;
    public int NumberOfBullets { get { return tankMovementData.NumberOfBullets; } }

    /*Rotate Head*/
    private float turnSmoothVelocity;
    private float TurnSmoothTime = 0.3f;

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;
        MousePosition = Vector3.forward;

        inputs = new bool[4];
        forceModifier = tankMovementData.MovementForce * Time.fixedDeltaTime;
        BulletObjectPool.Instance.CreateInstances(NumberOfBullets);
        bulletCount = 0;
        nextFire = 0f;
        isDead = false;
    }

    public void SetIsShooting(bool _isShooting)
    {
        isShooting = _isShooting;
    }

    public void FixedUpdate()
    {
        if (!isDead)
        {
            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.x -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
            }

            Move(_inputDirection);

            RotateHead(MousePosition);
        }
    }

    void Update()
    {
        if (isShooting && bulletCount < NumberOfBullets && Time.time > nextFire)
        {
            Fire();
            nextFire = Time.time + FireRate;
        }
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 force = (transform.right * _inputDirection.x + transform.forward * _inputDirection.y) * forceModifier;
        rb.AddForce(force);
        if (rb.velocity.magnitude > tankMovementData.MaximumVelocity)
        {
            rb.velocity = rb.velocity.normalized * tankMovementData.MaximumVelocity;
        }

        //Send to clients
        ServerSend.PlayerPosition(this);
    }

    void Fire()
    {
        Quaternion headPosition = headTransform.rotation;
        Vector3 velocity = (headPosition * Vector3.forward) * BulletVelocity;
        Vector3 position = headTransform.position + BulletDistanceOffset * (headPosition * Vector3.forward).normalized;

        GameObject b = BulletObjectPool.Instance.SpawnFromPool(position, headPosition, velocity, BulletVelocity, this, NumberOfBulletBounces);
    }

    private void RotateHead(Vector3 _mousePosition)
    {
        Vector3 direction = _mousePosition - headTransform.position;
        direction.y = 0;

        if (direction.magnitude >= 0.1f)
        {
            direction.Normalize();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            RotateHead(targetAngle);

            //Send to clients
            ServerSend.HeadRotation(this);
        }
    }

    private void RotateHead(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(headTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
        headTransform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void SetInput(bool[] _inputs, Vector3 _mousePosition)
    {
        inputs = _inputs;
        MousePosition = _mousePosition;
    }

    public void RemoveBullet()
    {
        bulletCount--;
        if (bulletCount < 0)
            Debug.LogWarning("Removed too many bullets");
    }

    public void AddBullet()
    {
        bulletCount++;
        if (bulletCount > NumberOfBullets)
            Debug.LogWarning("Added too many bullets");
    }
}
