﻿using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class BulletCollider : MonoBehaviour
{
    private float bulletVelocity;
    private int NumberOfBounces { get; set; }

    public Rigidbody rb;

    private TankFiringController owner;
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
                    bulletObejctPool.DestroyToPool(gameObject);
                }
                break;
            case "Player":
                NetworkManager.Instance.GetState().PlayerDeath(collision.collider.GetComponent<Player>());
                bulletObejctPool.DestroyToPool(gameObject);
                break;
            case "Tank":
                NetworkManager.Instance.GetState().EnemyDeath(collision.collider.GetComponent<Enemy>());
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

        ContactPoint contact = collision.GetContact(0);
        var curDir = rb.transform.forward;
        Vector3 newDir = Vector3.Reflect(curDir, contact.normal);
        rb.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        rb.velocity = Vector3.zero;
        rb.velocity = newDir.normalized * bulletVelocity;
    }

    public void OnBulletDespawn()
    {
        rb.velocity = Vector3.zero;
        bulletVelocity = 0f;
        currentBounces = 0;
        currentCollider = null;
        isChangingVelocity = false;

        ServerSend.DespawnBullet(Id);

        isActive = false;

        if (owner != null)
        {
            owner.RemoveBullet();
            owner = null;
        }
    }

    public void OnBulletSpawn(Vector3 _position, Quaternion _rotation, Vector3 velcoity, float maxVelocity, TankFiringController controller, int numberOfBounces)
    {
        //Make sure everything was reset properly
        currentBounces = 0;
        currentCollider = null;
        isChangingVelocity = false;

        transform.position = _position;
        transform.rotation = _rotation;
        NumberOfBounces = numberOfBounces;
        owner = controller;
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
