using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 MousePosition = Vector3.zero;

    private void Update()
    {
        ClientSend.PlayerShoot(Input.GetMouseButton(0));
        MousePosition = GetMousePosition();
        //if (isMouseDown)
        //    MousePosition = GetMousePosition();
    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
        };

        ClientSend.PlayerMovement(inputs, MousePosition);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            mousePos = hit.point;
            Debug.Log(mousePos);
        }
        return mousePos;
    }

    //private void RotateHead(Vector3 _mousePosition)
    //{
    //    Vector3 direction = _mousePosition - headTransform.position;
    //    direction.y = 0;

    //    if (direction.magnitude >= 0.1f)
    //    {
    //        direction.Normalize();
    //        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    //        RotateHead(targetAngle);
    //    }
    //}

    //private void RotateHead(float targetAngle)
    //{
    //    float angle = Mathf.SmoothDampAngle(headTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
    //    headTransform.rotation = Quaternion.Euler(0f, angle, 0f);
    //}
}
