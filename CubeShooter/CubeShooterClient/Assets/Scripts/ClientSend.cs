using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.Instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.Instance.udp.SendData(_packet);
    }
    
    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.Instance.myId);
            _packet.Write(UIManager.Instance.userNameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] inputs, Vector3 mousePos)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovment))
        {
            _packet.Write(inputs.Length);
            foreach(bool input in inputs)
            {
                _packet.Write(input);
            }
            _packet.Write(mousePos);

            SendUDPData(_packet);
        }
    }

    public static void PlayerShoot(bool isMouseButtonDown)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(isMouseButtonDown);

            SendTCPData(_packet);
        }
    }
    #endregion
}
