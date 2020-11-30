using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyFiringData enemyFiringData;
    [SerializeField] private Transform headTransform;
    public float MaxVisionDistance = 50f;
    public bool debug = false;
    public Color Color => enemyFiringData.GetBaseColor();

    public Player TargetedPlayer { get; private set; }

    public event Action<Player> UpdateTargetedPlayer;

    //Should make these come from a scriptable object?
    private const int SearchAngle = 180;
    private const int DegreeOffset = 1;
    private const float ClosestPlayerOffset = 10f;

    private Vector3 ShootDirection = Vector3.zero;
    private TankFiringController FiringController;

    // Start is called before the first frame update
    void Start()
    {
        FiringController = new TankFiringController(enemyFiringData, headTransform);
    }

    // Update is called once per frame
    private void Update()
    {
        //FindClosestPlayer();

        SetShooting();
        FiringController.Update();

        /*
         * if(CanMove)
         * {
         *      move();
         *      SendServer.SendPosition();
         * }    
         */
    }

    private void FixedUpdate()
    {
        ServerSend.HeadRotation(GetInstanceID(), headTransform.rotation, false);
    }

    public void Destroy()
    {
        ServerSend.DespawnEnemy(GetInstanceID());
        Destroy(gameObject);
    }

    private void FindClosestPlayer()
    {
        foreach (Player _player in FindObjectsOfType<Player>())
        { //TODO: improve performance ?
            if (!_player.GetIsDead())
            {
                if (TargetedPlayer == null)
                    SetTargetedPlayer(_player);
                else if (TargetedPlayer != _player)
                {
                    float nextPlayerDistance = Mathf.Abs(Vector3.Distance(transform.position, _player.transform.position));
                    float currentPlayerDistance = Mathf.Abs(Vector3.Distance(transform.position, TargetedPlayer.transform.position));
                    if (nextPlayerDistance + ClosestPlayerOffset < currentPlayerDistance)
                        SetTargetedPlayer(_player);
                }
            }
        }
    }

    private void SetShooting()
    {
        Vector3 headDirection = headTransform.forward;
        Vector3 pos = transform.position;

        if (CheckShot(pos, headDirection * MaxVisionDistance, 0, MaxVisionDistance))
        {
            FiringController.SetIsShooting(true);
        }
        else
        {
            FiringController.SetIsShooting(false);

            FindClosestPlayer(); //Find Closest player to set the TargetPlayer variable

            if(TargetedPlayer != null)
            {
                Vector3 playerPos = TargetedPlayer.transform.position;
                Vector3 direction = playerPos - pos;

                bool canSeePlayer = Physics.Raycast(pos, direction, out RaycastHit objectHit, MaxVisionDistance)
                    && objectHit.collider.CompareTag("Player");

                if (canSeePlayer)
                {
                    FiringController.RotateHead(playerPos);
                }
                else
                {
                    if (FindShot(direction))
                        FiringController.RotateHead(ShootDirection);
                }
            }
        }
    }

    private bool CheckShot(Vector3 from, Vector3 to, int currentNumberOfBounces, float distance)
    {
        bool hitPlayer = false;

        bool isHit = Physics.Raycast(from, to, out RaycastHit objectHit, distance);

        if (isHit)
        {
            string tag = objectHit.collider.tag;
            switch (tag)
            {
                case "Player":
                    hitPlayer = true;

                    if (debug)
                    {
                        var dir = objectHit.point - from;
                        Debug.DrawRay(from, dir, Color.blue);
                    }
                    break;
                case "Wall":
                    if (currentNumberOfBounces < enemyFiringData.NumberOfBulletBounces)
                    { //If the bullet can bounce check path of the bullet
                        float remDistance = distance - Vector3.Distance(from, objectHit.point);
                        Vector3 newFrom = objectHit.point;
                        Vector3 dir = objectHit.point - from;
                        Vector3 newTo = Vector3.Reflect(dir, objectHit.normal);

                        //Use recurision to follow path of the bullet
                        hitPlayer = CheckShot(newFrom, newTo, ++currentNumberOfBounces, remDistance);

                        if (debug && hitPlayer)
                        {
                            Debug.DrawRay(from, dir, Color.yellow);
                        }
                    }
                    break;
            }
        }
        if (hitPlayer)
        {
            ShootDirection = objectHit.point;
        }
        return hitPlayer;
    }

    private bool FindShot(Vector3 direction)
    {
        Vector3 from, to;
        bool hitPlayer = false;
        int angle;


        from = transform.position;
        int headAngle = Mathf.RoundToInt(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
        int startAngle = headAngle - SearchAngle;
        int endAngle = headAngle + SearchAngle;

        if (debug)
        {
            var startVector = Quaternion.Euler(0, startAngle, 0) * -direction;
            var endVector = Quaternion.Euler(0, endAngle, 0) * -direction;
            Debug.DrawRay(from, startVector, Color.black);
            Debug.DrawRay(from, endVector, Color.black);
        }


        //for every y degrees current angle -x degrees to current angle plus x degrees
        //ie (for every 5 degrees from curr_angle - 45 to curr_angle + 45)
        for(angle = startAngle; angle <= endAngle; angle += DegreeOffset)
        {
            to = Quaternion.Euler(0, angle, 0) * -direction;

            //Follow Raycast direction check if player is hit
            if(CheckShot(from, to, 0, MaxVisionDistance))
            {
                hitPlayer = true;
                break; //exit out of for loop
            }
        }

        return hitPlayer;
    }

    private void SetTargetedPlayer(Player _targetedPlayer)
    {
        TargetedPlayer = _targetedPlayer;
        UpdateTargetedPlayer?.Invoke(_targetedPlayer);
    }

    public TankFiringData GetFiringData() => enemyFiringData;

    public void SetFiringData(EnemyFiringData _enemyFiringData)
    {
        enemyFiringData = _enemyFiringData;
    }

    //public override void OnLoad()
    //{
    //    if(enemyFiringData.GetType().Name == "EnemyMovementData")
    //    {
    //        NetworkManager.Instance.SetBuildNavMesh(true);
    //        GetComponent<NavMeshAgent>().enabled = true;
    //        GetComponent<EnemyMovement>().enabled = true;
    //    }

    //    if(TryGetComponent(out EnemyMovement enemyMovement))
    //    {
    //        enemyMovement.OnLoad();
    //    }
    //}
}
