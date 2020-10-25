using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    [SerializeField]
    private BulletCollider prefab;
    private Queue<GameObject> bulletQueue;
    private int lastId = 1;
    private int bulletsToRemove;
    //

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

    public void CreateInstances(int instances)
    {
        for (int i = 0; i < instances; i++)
        {
            var _bullet = Instantiate(prefab);
            _bullet.CreateInstance(lastId++);

            bulletQueue.Enqueue(_bullet.gameObject);
        }
    }

    public void RemoveBulletsFromPlayer(Player player)
    {
        bool earlyBreak = false;
        int i;

        for(i = 0; i < player.NumberOfBullets; i++)
        {
            if (bulletQueue.Count == 0)
            {
                earlyBreak = true;
                break;
            }
            GameObject _bullet = bulletQueue.Dequeue();
            Destroy(_bullet);
        }

        if (earlyBreak)
        {
            bulletsToRemove = player.NumberOfBullets - i;
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation, Vector3 velocity, float maxVelocity, Player player, int numberOfBounces)
    {
        GameObject bullet = bulletQueue.Dequeue();
        bullet.SetActive(true);

        bullet.GetComponent<BulletCollider>().OnBulletSpawn(position, rotation, velocity, maxVelocity, player, numberOfBounces);
        return bullet;
    }

    public void DestroyToPool(GameObject bullet)
    {
        bullet.GetComponent<BulletCollider>().OnBulletDespawn();
        bullet.SetActive(false);
        if (bulletsToRemove-- > 0)
            Destroy(bullet);
        else
            bulletQueue.Enqueue(bullet);
    }
}
