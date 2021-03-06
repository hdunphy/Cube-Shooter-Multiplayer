﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            //Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            Debug.Log($"Player (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }

        Server.clients[_fromClient].ConnectToLobby();
        //Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Vector3 _mousePosition = _packet.ReadVector3();

        Server.clients[_fromClient].player.SetInput(_inputs, _mousePosition);
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Server.clients[_fromClient].player.SetIsShooting(_packet.ReadBool());
    }

    public static void UpdatePlayerInfo(int _fromClient, Packet _packet)
    {
        string username = _packet.ReadString();
        Color userColor = _packet.ReadColor();
        bool isReady = _packet.ReadBool();

        Server.clients[_fromClient].UpdateInfo(username, userColor, isReady);

        if (NetworkManager.Instance.AllPlayersReady())
        {
            //Need to allow user to change this
            NetworkManager.Instance.NextState();
        }
    }

    public static void ContinueWithGame(int _fromClient, Packet _packet)
    {
        var clients = Server.GetAllActiveClients();
        if (clients.Count() < 1) //probably not necessary
            Debug.LogWarning("No active clients left");

        if(_fromClient == clients.First().id)
        {
            bool _toContinue = _packet.ReadBool();
            if (_toContinue)
            {
                NetworkManager.Instance.LoadLevel();
            }
            else
            {
                NetworkManager.Instance.NextState();
            }
        }
        else
        {
            Debug.Log("Continue with Game Requested by a non Host");
        }
    }
}
