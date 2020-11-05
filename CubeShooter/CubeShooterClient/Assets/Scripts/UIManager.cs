using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startMenu;
    public GameObject connectMenu;
    public GameObject stage;
    public GameObject uiCamera;

    public InputField userNameField;
    public InputField ipAddressInput;
    public Button joinButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        ipAddressInput.onValueChanged.AddListener(delegate { EnableJoin(); });
    }

    public void ConfirmUserName()
    {
        userNameField.interactable = false;
        startMenu.SetActive(false);
        connectMenu.SetActive(true);
    }


    public void StartGame()
    {
        //startMenu.SetActive(false);
        //userNameField.interactable = false;
        stage.SetActive(true);
        uiCamera.SetActive(false);
        //Client.Instance.ConnectToServer();

        //Send into game
    }

    public void ConnectAsHost()
    {
        connectMenu.SetActive(false);
        ipAddressInput.interactable = false;
        startMenu.SetActive(true);
        Client.Instance.ConnectToServer();
    }

    public void ConnectAsJoin()
    {
        connectMenu.SetActive(false);
        ipAddressInput.interactable = false;
        startMenu.SetActive(true);
        Client.Instance.ConnectToServer(ipAddressInput.text);
    }

    private void EnableJoin()
    {
        joinButton.interactable = false;
        if (!string.IsNullOrEmpty(ipAddressInput.text))
        {
            //check if it is a valid IP Address
            if (IPAddress.TryParse(ipAddressInput.text, out IPAddress address) &&
                address.AddressFamily.Equals(AddressFamily.InterNetwork))
            {
                joinButton.interactable = true;
            }
        }
    }
}
