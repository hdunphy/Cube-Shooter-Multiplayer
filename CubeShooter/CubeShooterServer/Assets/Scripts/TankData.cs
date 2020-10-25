using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/Tanks/TankData")]
public class TankData : ScriptableObject
{
    [SerializeField] private float movementForce;
    [SerializeField] private float maximumVelcoity;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletDistanceOffset;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private int numberOfBulletBounces;
    [SerializeField] private int numberOfBullets;
    [SerializeField] private float respawnTime;

    public float MovementForce { get { return movementForce; } }
    public float MaximumVelocity { get { return maximumVelcoity; } }
    public float FireRate { get { return fireRate; } }
    public float BulletDistanceOffset { get { return bulletDistanceOffset; } }
    public float BulletVelocity { get { return bulletVelocity; } }
    public int NumberOfBulletBounces { get { return numberOfBulletBounces; } }
    public int NumberOfBullets { get { return numberOfBullets; } }

    public float RespawnTime { get { return respawnTime; } }
}
