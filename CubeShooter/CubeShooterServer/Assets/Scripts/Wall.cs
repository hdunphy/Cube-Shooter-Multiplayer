using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallSize { small, medium }

    public WallSize wallSize = WallSize.medium;

    public Vector3 WallPosition { get => GetPosition(); }

    private Vector3 GetPosition()
    {
        Vector3 _position = transform.position;

        switch (wallSize)
        {
            case WallSize.small:
                _position.y = -1;
                break;
            default:
                _position = transform.position;
                break;
        }

        return _position;
    }
}
