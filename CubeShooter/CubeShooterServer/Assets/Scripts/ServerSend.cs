using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    //Sends players list of all players
    public static void ConnectToLobby()
    {
        using (Packet _packet = new Packet((int)ServerPackets.connectToLobby))
        {
            var activeClients = Server.GetAllActiveClients();

            _packet.Write(activeClients.Count());
            Debug.Log($"Number of clients in lobby: {activeClients.Count()}");
            
            foreach(Client _client in activeClients)
            {
                _packet.Write(_client.id);
                _packet.Write(_client.userName);
                _packet.Write(_client.color);
                _packet.Write(_client.isReady);
            }

            SendTCPDataToAll(_packet);
        }
    }

    public static void UpdatePlayerInfo(Client _client)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updatePlayerInfo))
        {
            _packet.Write(_client.id);
            _packet.Write(_client.userName);
            _packet.Write(_client.color);
            _packet.Write(_client.isReady);

            SendTCPDataToAll(_client.id, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            //Player Data
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnWalls(int _id, List<Vector3> wallPositions)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnWalls))
        {
            //Level Data
            _packet.Write(wallPositions.Count);
            for (int i = 0; i < wallPositions.Count; i++)
                _packet.Write(wallPositions[i]);

            SendTCPData(_id, _packet);
        }
    }

    public static void TankPosition(int _id, Vector3 _position, bool _isPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.tankPosition))
        {
            _packet.Write(_isPlayer);
            _packet.Write(_id);
            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }

    }

    /// <summary>
    /// Send the turret head rotation of a tank (either player or enemy)
    /// </summary>
    /// <param name="_id">id of the tank either player.id or enemy.GetInstanceID()</param>
    /// <param name="_rotation">the current rotation</param>
    /// <param name="_isPlayer">if the tank is a player or an enemey</param>
    public static void HeadRotation(int _id, Quaternion _rotation, bool _isPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.headRotation))
        {
            _packet.Write(_isPlayer);
            _packet.Write(_id);
            _packet.Write(_rotation);

            SendUDPDataToAll(_packet);
        }

    }

    public static void PlayerRespawn(int _id, float _seconds)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRespawn))
        {
            _packet.Write(_id);
            _packet.Write(_seconds);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnect))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void BulletPosition(int _id, Transform _transform)
    {
        using (Packet _packet = new Packet((int)ServerPackets.bulletPosition))
        {
            _packet.Write(_id);
            _packet.Write(_transform.position);
            _packet.Write(_transform.rotation);

            SendUDPDataToAll(_packet);
        }
    }

    public static void DespawnBullet(int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.despawnBullet))
        {
            _packet.Write(_id);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnEnemies(int _id, Enemy[] _enemies)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            //Level Data
            _packet.Write(_enemies.Length);
            foreach(Enemy _enemy in _enemies)
            {
                _packet.Write(_enemy.GetInstanceID());
                _packet.Write(_enemy.transform.position);
                _packet.Write(_enemy.Color);
            }

            SendTCPData(_id, _packet);
        }
    }

    public static void DespawnEnemy(int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.despawnEnemy))
        {
            _packet.Write(_id);

            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
