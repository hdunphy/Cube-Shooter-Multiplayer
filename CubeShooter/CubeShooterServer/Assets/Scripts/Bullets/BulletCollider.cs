using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class BulletCollider : MonoBehaviour
{
    private float bulletVelocity;
    private int NumberOfBounces { get; set; }

    public Rigidbody rb;
    //[SerializeField] private VisualEffect smoke;

    private Player owner;
    private BulletObjectPool bulletObejctPool;
    private int currentBounces = 0;
    private bool isActive = false;
    private bool isChangingVelocity = false;
    private Collider currentCollider;
    private int Id;

    private void Start()
    {
        bulletObejctPool = BulletObjectPool.Instance;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (isActive)
        {
            if (rb.velocity == Vector3.zero)
                bulletObejctPool.DestroyToPool(gameObject);
            ServerSend.BulletPosition(Id, transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"BulletId: {Id}. Contacts: {collision.contactCount}. From collider {collision.collider.gameObject.GetInstanceID()}");

        string tag = collision.collider.tag;
        switch (tag)
        {
            case "Wall":
                if (isChangingVelocity)
                {
                    break;
                }
                else if (currentBounces++ < NumberOfBounces)
                    ChangeVelocity(collision);
                else
                {
                    //Debug.Log($"BulletId: {Id} destroyed by: {collision.collider.gameObject.GetInstanceID()} with bounces: {currentBounces}");
                    bulletObejctPool.DestroyToPool(gameObject);
                }
                break;
            //case "Player":
            case "Tank":
                collision.collider.GetComponent<Player>().Respawn();
                //collision.collider.gameObject.GetComponent<Explosion>().Explode(gameObject);
                bulletObejctPool.DestroyToPool(gameObject);
                break;
            case "Bullet":
                bulletObejctPool.DestroyToPool(gameObject);
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.Equals(currentCollider))
            isChangingVelocity = false;
    }

    private void ChangeVelocity(Collision collision)
    {
        isChangingVelocity = true;
        currentCollider = collision.collider;
        //Debug.Log($"BulletId: {Id} has {currentBounces} bounces left. From collider {collision.collider.gameObject.GetInstanceID()}");

        ContactPoint contact = collision.GetContact(0);
        var curDir = rb.transform.forward;
        Vector3 newDir = Vector3.Reflect(curDir, contact.normal);
        rb.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        rb.velocity = Vector3.zero;
        rb.velocity = newDir.normalized * bulletVelocity;
    }

    public void OnBulletDespawn()
    {
        if (owner != null)
        {
            owner.RemoveBullet();
            owner = null;
        }
        rb.velocity = Vector3.zero;
        bulletVelocity = 0f;
        currentBounces = 0;
        currentCollider = null;
        isChangingVelocity = false;

        ServerSend.DespawnBullet(Id);

        isActive = false;
    }

    public void OnBulletSpawn(Vector3 _position, Quaternion _rotation, Vector3 velcoity, float maxVelocity, Player player, int numberOfBounces)
    {
        transform.position = _position;
        transform.rotation = _rotation;
        NumberOfBounces = numberOfBounces;
        owner = player;
        owner.AddBullet();

        rb.velocity = velcoity;
        bulletVelocity = maxVelocity;

        isActive = true;
    }

    public void CreateInstance(int _id)
    {
        gameObject.SetActive(false);
        Id = _id;
    }

    public Player GetOwner()
    {
        return owner;
    }
}
