using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public GameObject playerPrefab;
    [SerializeField] private LevelSetUp levelSetUp;

    private GameStateMachine StateMachine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(20, 26950);
        StateMachine = new GameStateMachine(new Dictionary<StateType, IGameState>()
        {
            {StateType.Lobby, new LobbyState() },
            {StateType.Classic, new ClassicState() },
            {StateType.ArenaBattle, new ArenaBattleState() }
        });
    }

    private void OnApplicationQuit()
    {
        //TODO: Send Stop to client
        Server.Stop();
    }

    public List<Vector3> GetWallPositions()
    {
        return FindObjectsOfType<Wall>().Select(x => x.WallPosition).ToList();
    }

    public Enemy[] GetEnemies()
    {
        return FindObjectsOfType<Enemy>();
    }
    public void SetBuildNavMesh(bool _setNavMesh)
    {
        levelSetUp.SetBuildNaveMesh(_setNavMesh);
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, RespawnLocation.Instance.GetRespawnLocation(), Quaternion.identity).GetComponent<Player>();
    }

    public bool AllPlayersReady()
    {
        return Server.GetAllActiveClients().Any(x => x.isReady); //WHere is connected and is not ready
    }

    public void LoadLevel()
    {
        levelSetUp.LoadLevelFromPNG();
        foreach (Client _client in Server.GetAllActiveClients())
            _client.SendIntoGame();
    }

    public bool SetLevelIndex(int levelIndex)
    {
        return levelSetUp.SetLevelIndex(levelIndex);
    }

    public void ResetLevel(bool isSuccess)
    {
        foreach (Client _client in Server.GetAllActiveClients())
        {
            //BulletObjectPool.Instance.RemoveBulletsFromPlayer(_client.player);
            Destroy(_client.player.gameObject);
        }

        levelSetUp.ResetLevel();

        BulletObjectPool.Instance.DestroyAllBullets();

        //SendServer to return to lobby
        ServerSend.EndLevel(isSuccess);
    }

    public IGameState GetState()
    {
        return StateMachine.CurrentState;
    }

    public void NextState()
    {
        StateMachine.UpdateState();
    }
}
