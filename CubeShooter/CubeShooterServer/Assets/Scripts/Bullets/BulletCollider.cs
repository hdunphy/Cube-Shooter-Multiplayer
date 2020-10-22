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
        string tag = collision.collider.tag;
        switch (tag)
        {
            case "Wall":
                if (currentBounces++ < NumberOfBounces)
                    ChangeVelocity(collision);
                else
                    bulletObejctPool.DestroyToPool(gameObject);
                break;
            //case "Player":
            case "Tank":
                //collision.collider.gameObject.GetComponent<Explosion>().Explode(gameObject);
                bulletObejctPool.DestroyToPool(gameObject);
                break;
            case "Bullet":
                bulletObejctPool.DestroyToPool(gameObject);
                break;
        }
    }

    private void ChangeVelocity(Collision collision)
    {
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

        //effect.Stop();
        //effect.transform.parent = null;

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
}
