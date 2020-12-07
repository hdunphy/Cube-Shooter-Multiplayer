using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, BulletManager> bullets = new Dictionary<int, BulletManager>();
    public static Dictionary<int, EnemyManager> enemies = new Dictionary<int, EnemyManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject wallPrefab;
    public Transform wallParent;
    public BulletManager bulletPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _roatation)
    {
        GameObject _player;
        if (_id == Client.Instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _roatation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _roatation);
        }

        _player.GetComponent<PlayerManager>().SpawnPlayer(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnEnemy(int _id, Vector3 _position, Color _color)
    {
        GameObject _enemy = Instantiate(enemyPrefab, _position, Quaternion.identity);
        EnemyManager enemyMan = _enemy.GetComponent<EnemyManager>();
        enemyMan.Renderer.material.SetColor("_Color", _color);
        enemies.Add(_id, enemyMan);
    }

    public void SpawnWalls(Vector3[] wallPositions)
    {
        for(int i = 0; i < wallPositions.Length; i++)
        {
            Instantiate(wallPrefab, wallPositions[i], Quaternion.identity, wallParent);
        }
    }

    public void MoveBullet(int _id, Vector3 _position, Quaternion _rotation)
    {
        if (!bullets.ContainsKey(_id))
        {
            bullets.Add(_id, Instantiate(bulletPrefab, _position, _rotation));
        }
        else
        {
            bullets[_id].MoveBullet(_position, _rotation);
        }
    }

    public void EndLevel(bool isSuccessful)
    {
        UIManager.Instance.ShowLevelCompletePopup(isSuccessful);

        //Destroy all walls
        for (int i = 0; i < wallParent.childCount; i++)
            Destroy(wallParent.GetChild(i).gameObject);

        //Destroy all bullets
        foreach (BulletManager _bullet in bullets.Values)
        {
            Destroy(_bullet.gameObject);
        }
        bullets.Clear();

        //Destroy all enemies
        foreach (EnemyManager _enemy in enemies.Values)
            Destroy(_enemy.gameObject);
        enemies.Clear();

        //Destroy all players
        foreach (PlayerManager _player in FindObjectsOfType<PlayerManager>())
        {
            players.Remove(_player.id);
            Destroy(_player.gameObject);
        }
    }
}
