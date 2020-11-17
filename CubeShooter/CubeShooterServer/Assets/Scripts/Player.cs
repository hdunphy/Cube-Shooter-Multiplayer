using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    //TODO: Remove this?
    public string username;

    [SerializeField] private TankFiringData FiringData;
    [SerializeField] private Rigidbody rb;
    public Transform headTransform;
    private bool isDead;

    [SerializeField] private float MovementForce = 500;
    [SerializeField] private float RespawnTime = 3;
    [SerializeField] private float MaximumVelocity = 3.5f;
    private float forceModifier;
    private bool[] inputs;
    private Vector3 MousePosition;
    private TankFiringController FiringController;

    public void Initialize(int id, string username, Color color)
    {
        this.id = id;
        this.username = username; //TODO: Remove Username
        MousePosition = Vector3.forward;

        inputs = new bool[4];
        forceModifier = MovementForce * Time.fixedDeltaTime;
        FiringController = new TankFiringController(FiringData, headTransform);

        isDead = false;
    }

    public int GetNumberOfBullets()
    {
        return FiringController.NumberOfBullets;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine(RespawnTime));
    }

    private IEnumerator RespawnCoroutine(float seconds)
    {
        ServerSend.PlayerRespawn(id, seconds);
        isDead = true;
        GameObject tankObject = transform.GetChild(0).gameObject;
        tankObject.SetActive(false);

        yield return new WaitForSeconds(seconds);

        transform.position = RespawnLocation.Instance.GetRespawnLocation();
        tankObject.SetActive(true);

        while(transform.position.y > 0.1)
        {
            Move(Vector2.zero);
            yield return null;
        }

        isDead = false;
    }

    public void SetIsShooting(bool _isShooting)
    {
        FiringController.SetIsShooting(_isShooting);
    }

    public void FixedUpdate()
    {
        if (!isDead)
        {
            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.x -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
            }

            Move(_inputDirection);

            if (FiringController.RotateHead(MousePosition))
                ServerSend.HeadRotation(this);
        }
    }

    void Update()
    {
        if (!isDead)
        {
            FiringController.Update();
        }
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 force = (transform.right * _inputDirection.x + transform.forward * _inputDirection.y) * forceModifier;
        rb.AddForce(force);
        if (rb.velocity.magnitude > MaximumVelocity)
        {
            rb.velocity = rb.velocity.normalized * MaximumVelocity;
        }

        //Send to clients
        ServerSend.PlayerPosition(this);
    }

    public void SetInput(bool[] _inputs, Vector3 _mousePosition)
    {
        inputs = _inputs;
        MousePosition = _mousePosition;
    }
}
