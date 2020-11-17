using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankFiringData", menuName = "ScriptableObjects/Tanks/TankFiringData")]
public class TankFiringData : ScriptableObject
{
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletDistanceOffset;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private int numberOfBulletBounces;
    [SerializeField] private int numberOfBullets;
    [SerializeField] private float turnSmoothTime;
    public float FireRate { get { return fireRate; } }
    public float BulletDistanceOffset { get { return bulletDistanceOffset; } }
    public float BulletVelocity { get { return bulletVelocity; } }
    public int NumberOfBulletBounces { get { return numberOfBulletBounces; } }
    public int NumberOfBullets { get { return numberOfBullets; } }
    public float TurnSmoothTime { get { return turnSmoothTime; } }

}
