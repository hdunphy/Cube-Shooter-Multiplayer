﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMovementData", menuName = "ScriptableObjects/Tanks/EnemyMovementData")]
public class EnemyMovementData : EnemyFiringData
{
    [SerializeField] private float navMeshAngularSpeed;
    [SerializeField] private float navMeshAcceleration;
    [SerializeField] private float navMeshVelocity;
    [SerializeField] private float strafeDistance;
    [SerializeField] private float chaseDistance;

    public float NavMeshAngularSpeed { get => navMeshAngularSpeed; }
    public float NavMeshAcceleration { get => navMeshAcceleration; }
    public float NavMeshVelocity { get => navMeshVelocity; }
    public float StrafeDistance { get => strafeDistance; }
    public float ChaseDistance { get => chaseDistance; }

    public override void OnLoad(Vector3 _position, Transform _parent)
    {
        base.OnLoad(_position, _parent);
        NetworkManager.Instance.SetBuildNavMesh(true);
    }
}
