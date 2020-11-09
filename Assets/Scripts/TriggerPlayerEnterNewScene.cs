using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class TriggerPlayerEnterNewScene : MonoBehaviour
{
    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;

    }
    /// <summary>
    /// NETWORKING BELOW
    /// </summary>
    private void OnDestroy()
    {
        LocalPlayerAnnouncer.OnLocalPlayerUpdated -= PlayerUpdated;
    }
    private void PlayerUpdated(NetworkIdentity localPlayer)
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<NetworkPlayerController>().EnterNewScene();
        }
        //this.enabled = (localPlayer != null);
    }
}
