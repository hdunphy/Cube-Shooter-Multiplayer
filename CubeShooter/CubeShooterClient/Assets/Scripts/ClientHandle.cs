using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.Instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.Instance.udp.Connect(((IPEndPoint)Client.Instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void ConnectToLobby(Packet _packet)
    {
        PlayerObject[] players = new PlayerObject[_packet.ReadInt()];

        for (int i = 0; i < players.Length; i++)
        {
            int _id = _packet.ReadInt();
            string _userName = _packet.ReadString();
            Color _color = _packet.ReadColor();
            bool _isReady = _packet.ReadBool();

            players[i] = new PlayerObject(_id, _userName, _color, _isReady);
        }

        UIManager.Instance.SetPlayerObjects(players);
        UIManager.Instance.LoadLobby();
    }

    public static void UpdatePlayerInfo(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string username = _packet.ReadString();
        Color _color = _packet.ReadColor();
        bool _isReady = _packet.ReadBool();

        UIManager.Instance.UpdatePlayerObject(_id, username, _color, _isReady);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        //Player data
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        UIManager.Instance.StartGame();

        //Spawn
        GameManager.Instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void SpawnWalls(Packet _packet)
    {
        //Level data
        Vector3[] _wallPositions = new Vector3[_packet.ReadInt()];
        for (int i = 0; i < _wallPositions.Length; i++)
        {
            _wallPositions[i] = _packet.ReadVector3();
        }

        GameManager.Instance.SpawnWalls(_wallPositions);
    }

    public static void TankPosition(Packet _packet)
    {
        bool _isPlayer = _packet.ReadBool();
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();


        if (_isPlayer)
        {
            if (GameManager.players.TryGetValue(_id, out PlayerManager playerManager))
                playerManager.transform.position = _position;
            else
                DictionaryMissingKeyError("Player", _id);
        }
        else
        {
            if (GameManager.enemies.TryGetValue(_id, out EnemyManager enemyManager))
                enemyManager.transform.position = _position;
            else
                DictionaryMissingKeyError("Enemy", _id);
        }
    }

    public static void SetTankActive(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isActive = _packet.ReadBool();
        bool _isPlayer = _packet.ReadBool();

        if (_isPlayer)
        {
            if (GameManager.players.TryGetValue(_id, out PlayerManager playerManager))
                playerManager.SetTankActive(_isActive);
            else
                DictionaryMissingKeyError("Player", _id);
        }
        else
        {
            if (GameManager.enemies.TryGetValue(_id, out EnemyManager enemyManager))
                enemyManager.SetTankActive(_isActive);
            else
                DictionaryMissingKeyError("Enemy", _id);
        }
    }

    public static void HeadRotation(Packet _packet)
    {
        bool isPlayer = _packet.ReadBool();
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (isPlayer)
        {
            if (GameManager.players.TryGetValue(_id, out PlayerManager playerManager))
                playerManager.headTransform.rotation = _rotation;
            else
                DictionaryMissingKeyError("Player", _id);
        }
        else
        {
            if (GameManager.enemies.TryGetValue(_id, out EnemyManager enemyManager))
                enemyManager.headTransform.rotation = _rotation;
            else
                DictionaryMissingKeyError("Enemy", _id);
        }
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        UIManager.Instance.RemovePlayerObject(_id);

        if (GameManager.players.ContainsKey(_id))
        {
            Destroy(GameManager.players[_id].gameObject);
            GameManager.players.Remove(_id);
        }
        else
            DictionaryMissingKeyError("Player", _id);
    }

    public static void BulletPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.Instance.MoveBullet(_id, _position, _rotation);
    }

    public static void DespawnBullet(Packet _packet)
    {
        int _id = _packet.ReadInt();

        //Destroy(GameManager.bullets[_id].gameObject);
        GameManager.bullets[_id].Despawn();
        GameManager.bullets.Remove(_id);
    }

    public static void SpawnEnemy(Packet _packet)
    {
        int id;
        Vector3 position;
        Color color;

        int count = _packet.ReadInt();
        Debug.Log($"Found {count} enemies");

        for(int i = 0; i < count; i++)
        {
            id = _packet.ReadInt();
            position = _packet.ReadVector3();
            color = _packet.ReadColor();
            GameManager.Instance.SpawnEnemy(id, position, color);
        }
    }

    public static void DespawnEnemy(Packet _packet)
    {
        int _id = _packet.ReadInt();

        if(GameManager.enemies.TryGetValue(_id, out EnemyManager enemyManager))
        {
            Destroy(enemyManager.gameObject);
            GameManager.enemies.Remove(_id);
        }
        else
        {
            DictionaryMissingKeyError("Enemy", _id);
        }
    }

    private static void DictionaryMissingKeyError(string type, int id)
    {
        Debug.LogWarning($"{type} {id} not found in dictionary");
    }
}
