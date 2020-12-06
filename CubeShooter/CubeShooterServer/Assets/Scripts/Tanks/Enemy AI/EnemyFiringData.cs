using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFiringData", menuName = "ScriptableObjects/Tanks/EnemyFiringData")]
public class EnemyFiringData : TankFiringData
{
    [SerializeField] private Enemy EnemyPrefab;

    public override void OnLoad(Vector3 _position, Transform _parent)
        {
        EnemyPrefab.SetFiringData(this);
        Enemy enemy = Instantiate(EnemyPrefab, _position, Quaternion.identity, _parent);
    }
}
