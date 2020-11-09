using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using System.Linq;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator animator = null;

    private MyNetworkManagerLobby room;

    private MyNetworkManagerLobby Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton as MyNetworkManagerLobby;
        }
    }

    public void CountdownEnded()
    {
        animator.enabled = false;
    }

    #region Server

    public override void OnStartServer()
    {
        MyNetworkManagerLobby.OnServerStopped += CleanUpServer;
        MyNetworkManagerLobby.OnServerReadied += CheckToStartRound;

    }

    [ServerCallback]

    private void OnDestroy()
    {
        CleanUpServer();
    }


    [Server]

    private void CleanUpServer()
    {
        MyNetworkManagerLobby.OnServerStopped -= CleanUpServer;
        MyNetworkManagerLobby.OnServerReadied -= CheckToStartRound;
    }

    [ServerCallback]

    public void StartRound()
    {
        RpcStartRound();
    }

    [Server]
    private void CheckToStartRound(NetworkConnection conn)
    {
        if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { return; }
        //if the number of gameplays that are ready is not equal to the num of GP then return 
        animator.enabled = true;

        RpcStartCountdown();

    }
    #endregion

    #region Client

    [ClientRpc]
    private void RpcStartCountdown()
    {
        animator.enabled = true;
        foreach (var player in Room.GamePlayers)
        {
            if (player.hasAuthority)
            {
                player.CmdUpdateDisplayInitial();
                break;
            }
        }
    }

    [ClientRpc]

    private void RpcStartRound()
    {
        Debug.Log("RpcStartRound()");
        foreach (var player in Room.GamePlayers)
        {
            if (player.hasAuthority)
            {
                player.gameObject.GetComponent<NetworkPlayerController>().isDying = false;
                break;
            }
        }
    }
    #endregion 


}
