using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private List<EnemyAIStateType> EnemyStates;
    [SerializeField] private NearestBulletDetector bulletDetector;
    public float StrafeDistance => MovementData.StrafeDistance;
    public float ChaseDistance => MovementData.ChaseDistance;
    public Vector3? TargetDestination { get; set; }
    public Player TargetedPlayer { get; private set; }
    public BulletCollider NearestBullet { get; private set; }

    private NavMeshAgent navMeshAgent;
    private Enemy enemyController;
    private EnemyMovementData MovementData;

    private EnemyAIStateMachine stateMachine;

    private void Start()
    {
        //Set up state machine
        Dictionary<EnemyAIStateType, BaseState> stateMachineDictionary = new Dictionary<EnemyAIStateType, BaseState>();
        foreach (EnemyAIStateType state in EnemyStates)
        {
            stateMachineDictionary.Add(state, StateFactory.CreateBaseState(state, this));
        }
        stateMachine = new EnemyAIStateMachine(stateMachineDictionary);
        TargetDestination = null;

        enemyController = GetComponent<Enemy>();
        MovementData = (EnemyMovementData)enemyController.GetFiringData();

        navMeshAgent = GetComponent<NavMeshAgent>();

        //navMeshAgent.updateRotation = false;
        navMeshAgent.acceleration = MovementData.NavMeshAcceleration;// 8; //enemy
        navMeshAgent.angularSpeed = MovementData.NavMeshAngularSpeed;//120; // enemy
        navMeshAgent.speed = MovementData.NavMeshVelocity; //3; //

        //Events
        enemyController.UpdateTargetedPlayer += EnemyController_UpdateTargetedPlayer;
        bulletDetector.AddDangerousBullet += BulletDetector_AddDangerousBullet;
    }

    private void Update()
    {
        //if (NearestBullet != null)
        //    stateMachine.SetToAvoid;
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (TargetDestination.HasValue)
            navMeshAgent.SetDestination(TargetDestination.Value);

        ServerSend.TankPosition(enemyController.GetInstanceID(), transform.position, transform.rotation, false);
    }

    private void OnDestroy()
    {
        enemyController.UpdateTargetedPlayer -= EnemyController_UpdateTargetedPlayer;
        bulletDetector.AddDangerousBullet -= BulletDetector_AddDangerousBullet;
    }

    //public override void OnLoad()
    //{
    //    NetworkManager.Instance.SetBuildNavMesh(true);
    //}

    private void EnemyController_UpdateTargetedPlayer(Player _player) { TargetedPlayer = _player; }

    private void BulletDetector_AddDangerousBullet(BulletCollider _bullet) { NearestBullet = _bullet; }
}
