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

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRespawn(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _seconds = _packet.ReadFloat();

        GameManager.players[_id].Respawn(_seconds);
    }

    public static void HeadRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].headTransform.rotation = _rotation;
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
            Debug.Log($"Player {_id} missing from players dictionary");
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
}
