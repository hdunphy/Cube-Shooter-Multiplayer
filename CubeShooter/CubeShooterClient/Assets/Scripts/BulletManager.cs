using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
        //Add Visual Effects
    }

    public void Despawn()
    {
        //Remove visual effects
        gameObject.SetActive(false);
    }

    public void MoveBullet(Vector3 _position, Quaternion _rotation)
    {
        transform.position = _position;
        transform.rotation = _rotation;
    }
}
