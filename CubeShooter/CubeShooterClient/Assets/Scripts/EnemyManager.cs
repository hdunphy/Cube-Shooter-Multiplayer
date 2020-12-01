using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int Id;
    public Transform headTransform;
    public Renderer Renderer;
    public GameObject tankModel;

    public void SetTankActive(bool _isActive)
    {
        tankModel.SetActive(_isActive);
    }
}
