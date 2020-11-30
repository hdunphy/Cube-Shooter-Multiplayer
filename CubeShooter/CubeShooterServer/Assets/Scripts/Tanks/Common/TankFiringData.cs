using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankFiringData", menuName = "ScriptableObjects/Tanks/TankFiringData")]
public class TankFiringData : LoadableGameObject
{
    [SerializeField] private float fireRate;
    //[SerializeField] private float bulletDistanceOffset;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private int numberOfBulletBounces;
    [SerializeField] private int numberOfBullets;
    [SerializeField] private float turnSmoothTime;
    [SerializeField] private Color baseColor;
    public float FireRate { get { return fireRate; } }
    //public float BulletDistanceOffset { get { return bulletDistanceOffset; } }
    public float BulletVelocity { get { return bulletVelocity; } }
    public int NumberOfBulletBounces { get { return numberOfBulletBounces; } }
    public int NumberOfBullets { get { return numberOfBullets; } }
    public float TurnSmoothTime { get { return turnSmoothTime; } }

    public override Color GetBaseColor()
    {
        return baseColor;
    }

    public override void OnLoad(Vector3 _position, Transform parent)
    {
        throw new System.NotImplementedException();
    }
}
