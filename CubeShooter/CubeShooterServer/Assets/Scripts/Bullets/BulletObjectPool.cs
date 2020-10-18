using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private Queue<GameObject> bulletQueue;

    public static BulletObjectPool Instance;

    private void Awake()
    {
        bulletQueue = new Queue<GameObject>();
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //public void Awake()
    //{
    //    bulletQueue = new Queue<GameObject>();
    //    //Singleton
    //    Instance = this;
    //}

    public void CreateInstances(int instances)
    {
        for (int i = 0; i < instances; i++)
        {
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            bulletQueue.Enqueue(obj);
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation, Vector3 velocity, float maxVelocity, Player player, int numberOfBounces)
    {
        GameObject bullet = bulletQueue.Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.GetComponent<BulletCollider>().OnBulletSpawn(velocity, maxVelocity, player, numberOfBounces);
        return bullet;
    }

    public void DestroyToPool(GameObject bullet)
    {
        bullet.GetComponent<BulletCollider>().OnBulletDespawn();
        bullet.SetActive(false);
        bulletQueue.Enqueue(bullet);
    }
}
