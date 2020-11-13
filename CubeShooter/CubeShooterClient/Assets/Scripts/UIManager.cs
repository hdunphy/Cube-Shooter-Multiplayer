using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Objects")]
    public GameObject startMenu;
    public GameObject lobbyMenu;
    public GameObject connectPopup;


    [SerializeField] private Button HostButton;
    [SerializeField] private Button JoinButton;
    [SerializeField] private Button ConnectButton;
    [SerializeField] private TMP_InputField IpAddressInput;
    [SerializeField] private Color UnselectedColor;
    [SerializeField] private Color SelectedColor;

    private ColorBlock UnselectedColorBlock;
    private ColorBlock SelectedColorBlock;
    private const string MyIpAddress = "127.0.0.1";
    //Gameplay objects
    //private GameObject stage;
    //private GameObject uiCamera;

    //private InputField userNameField;
    //private InputField ipAddressInput;
    //private Button joinButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        SelectedColorBlock = new ColorBlock() { 
            normalColor = SelectedColor,
            selectedColor = SelectedColor,
            pressedColor = SelectedColor,
            highlightedColor = SelectedColor,
            colorMultiplier = 1,
            fadeDuration = .1f
        };
        UnselectedColorBlock = new ColorBlock()
        {
            normalColor = UnselectedColor,
            selectedColor = UnselectedColor,
            pressedColor = UnselectedColor,
            highlightedColor = UnselectedColor,
            colorMultiplier = 1,
            fadeDuration = .1f
        };
        //ipAddressInput.onValueChanged.AddListener(delegate { EnableJoin(); });
    }

    #region StartMenu Functions
    public void PlayButton()
    {
        PressJoinButton();
        connectPopup.SetActive(true);
    }

    public void MapEditorButton()
    {
        Debug.Log("Map Editor");
    }

    public void OptionsButton()
    {
        Debug.Log("Options");
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    #endregion

    #region Connect Popup Functions
    public void PressHostButton()
    {
        JoinButton.colors = UnselectedColorBlock;
        HostButton.colors = SelectedColorBlock;
        IpAddressInput.interactable = false;
        IpAddressInput.text = MyIpAddress;
    }

    public void PressJoinButton()
    {
        JoinButton.colors = SelectedColorBlock;
        HostButton.colors = UnselectedColorBlock;
        IpAddressInput.interactable = true;
        IpAddressInput.text = "";
    }

    public void PressPopupCancel()
    {
        connectPopup.SetActive(false);
    }

    public void PressConnectButton()
    {
        Client.Instance.ConnectToServer(IpAddressInput.text);
    }

    public void EnableJoin()
    {
        ConnectButton.interactable = false;
        if (!string.IsNullOrEmpty(IpAddressInput.text))
        {
            //check if it is a valid IP Address
            if (IPAddress.TryParse(IpAddressInput.text, out IPAddress address) &&
                address.AddressFamily.Equals(AddressFamily.InterNetwork))
            {
                ConnectButton.interactable = true;
            }
        }
    }
    #endregion

    //OLD CODE
    /*
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
    */
}
