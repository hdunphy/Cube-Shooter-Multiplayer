using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFiringController
{
    private TankFiringData FiringData;

    public int NumberOfBullets => FiringData.NumberOfBullets;
    private float BulletVelocity => FiringData.BulletVelocity;
    private float FireRate => FiringData.FireRate;
    private int NumberOfBulletBounces => FiringData.NumberOfBulletBounces;
    private float TurnSmoothTime => FiringData.TurnSmoothTime;

    private Transform transform;

    private float turnSmoothVelocity;

    private bool isShooting = false;
    private float nextFire;
    private int BulletCount;
    private const float BulletDistanceOffset = 1.5f;

    public TankFiringController(TankFiringData _firingData, Transform _transform)
    {
        FiringData = _firingData;
        transform = _transform;
        BulletCount = 0;
        nextFire = 0f;

        BulletObjectPool.Instance.CreateInstances(NumberOfBullets);
    }

    // Update is called once per frame
    public void Update()
    {
        if (isShooting && BulletCount < NumberOfBullets && Time.time > nextFire)
        {
            Fire();
            nextFire = Time.time + FireRate;
        }
    }

    void Fire()
    {
        Quaternion headPosition = transform.rotation;
        Vector3 velocity = (headPosition * Vector3.forward) * BulletVelocity;
        Vector3 position = transform.position + BulletDistanceOffset * (headPosition * Vector3.forward).normalized;

        GameObject b = BulletObjectPool.Instance.SpawnFromPool(position, headPosition, velocity, BulletVelocity, this, NumberOfBulletBounces);
    }

    public bool RotateHead(Vector3 hitPoint)
    {
        bool isRotating = false;

        Vector3 direction = hitPoint - transform.position;
        direction.y = 0;

        if (direction.magnitude >= 0.1f)
        {
            direction.Normalize();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            RotateHead(targetAngle);

            isRotating = true;
        }
        return isRotating;
    }

    public void RotateHead(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void SetIsShooting(bool isShooting)
    {
        this.isShooting = isShooting;
    }

    public void RemoveBullet()
    {
        BulletCount--;
        if (BulletCount < 0)
            Debug.LogWarning("Removed too many bullets");
    }

    public void AddBullet()
    {
        BulletCount++;
        if (BulletCount > NumberOfBullets)
            Debug.LogWarning("Added too many bullets");
    }
}
