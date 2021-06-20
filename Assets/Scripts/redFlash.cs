using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class redFlash : MonoBehaviour
{
    public int numFlashes;
    public float timeBetweenFlash;
    public Color flashColor = Color.red;
    public RawImage RedImage;
    private Color defaultColor;
    private GameObject player;

    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;
    }


    private void Start()
    {
        RedImage = gameObject.GetComponent<RawImage>();
    }
    private void OnEnable()
    {
        StartCoroutine(Flash());
    }
    IEnumerator Flash()
    {
        defaultColor = RedImage.color;

        for (int i = 0; i < numFlashes; i++)
        {
            // if the current color is the default color - change it to the flash color
            if (RedImage.color == defaultColor)
            {
                RedImage.color = flashColor;
            }
            else // otherwise change it back to the default color
            {
                RedImage.color = defaultColor;
            }
            yield return new WaitForSeconds(timeBetweenFlash);
        }
        //RedImage.enabled = true;
        gameObject.SetActive(false); // magic door closes - remove object
        yield return new WaitForSeconds(1);
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
            player = localPlayer.gameObject;
            player.GetComponent<PlayerEntity>().redFlash = gameObject;
        }
        //this.enabled = (localPlayer != null);
    }
}
