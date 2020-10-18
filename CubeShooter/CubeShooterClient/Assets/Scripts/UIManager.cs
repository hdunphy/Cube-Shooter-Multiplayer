using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startMenu;
    public GameObject stage;
    public GameObject uiCamera;
    public InputField userNameField;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        userNameField.interactable = false;
        stage.SetActive(true);
        uiCamera.SetActive(false);
        Client.Instance.ConnectToServer();
    }
}
