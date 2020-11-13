using System;
using System.Collections;
using System.Collections.Generic;
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
            _packet.Write(Server.clients.Count);
            
            foreach(int _id in Server.clients.Keys)
            {
                Client _client = Server.clients[_id];
                Vector3 color = new Vector3(_client.color.r, _client.color.g, _client.color.b);

                _packet.Write(_id);
                _packet.Write(_client.userName);
                _packet.Write(color);
            }

            SendTCPDataToAll(_packet);
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

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }

    }

    public static void HeadRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.headRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.headTransform.rotation);

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
    #endregion
}
