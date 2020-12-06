using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private VisualEffect smoke;
    private VisualEffect effect;

    private void Awake()
    {
        //Add Visual Effects

        effect = Instantiate(smoke, transform);
        effect.Play();
    }

    public void Despawn()
    {
        effect.Stop();
        effect.transform.parent = null;

        Destroy(effect.gameObject, 5);

        Destroy(this.gameObject);
    }

    public void MoveBullet(Vector3 _position, Quaternion _rotation)
    {
        transform.position = _position;
        transform.rotation = _rotation;
    }
}
