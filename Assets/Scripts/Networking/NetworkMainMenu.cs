using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class NetworkMainMenu : MonoBehaviour
{
    [SerializeField] private MyNetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private TelepathyTransport telepathyTrans = null;


    public void HostLobby()
    {
        networkManager.StartHost();
    }
    public void SetPort(string portNum)
    {
        telepathyTrans.port = Convert.ToUInt16(portNum);
    }
}
