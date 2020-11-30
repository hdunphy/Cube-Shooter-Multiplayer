using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadableGameObject : ScriptableObject
{
    public abstract void OnLoad(Vector3 _position, Transform parent);
    public abstract Color GetBaseColor();
}
