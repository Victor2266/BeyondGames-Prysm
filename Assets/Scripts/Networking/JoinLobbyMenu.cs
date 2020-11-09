using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private MyNetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        MyNetworkManagerLobby.OnClientConnected += HandleClientConnected;
        MyNetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }
    private void OnDisable()
    {
        MyNetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        MyNetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        
        string ipAddress = ipAddressInputField.text;

        if (ipAddressInputField.text.Length == 0)
        {
            ipAddress = "localhost";
        }

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);

        landingPagePanel.SetActive(false);

    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
