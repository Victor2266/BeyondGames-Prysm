using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DelayedCameraController : MonoBehaviour
{

    public Vector2 velocity;

    public bool SetTargetInInspector;

    public float smoothTimeY;

    public float smoothTimeX;

    public float offsetX;

    public float offsetY;

    public GameObject cameraTarget;

    public GameObject truePlayer;

    public GameObject mouseTarget;

    public float delay;

    public bool active;

    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;

        OptionsMenu.onCameraLockChangedCallback += UpdateCameraTarget;
        
    }
    private void Start()
    {
        if (!SetTargetInInspector)
        {
            truePlayer = GameObject.FindGameObjectWithTag("Player");
            mouseTarget = GameObject.FindGameObjectWithTag("Mouse");
            UpdateCameraTarget();
        }
        //is always delayed
        StartCoroutine(DelayCamera(delay));
    }

    private void FixedUpdate()
    {
        if (active)
        {
            float x = Mathf.SmoothDamp(transform.position.x, cameraTarget.transform.position.x + offsetX, ref velocity.x, smoothTimeX);
            float num = Mathf.SmoothDamp(transform.position.y, cameraTarget.transform.position.y + offsetY, ref velocity.y, smoothTimeY);
            transform.position = new Vector3(x, num + 0.5f, transform.position.z);
            /*if (!this.notTrackingPlayer)
            {
                if (this.player.GetComponent<Rigidbody2D>().velocity.x > 0.5f)
                {
                    this.offsetX = PlayerController.speed / 2f;
                }
                if (this.player.GetComponent<Rigidbody2D>().velocity.x < -0.5f)
                {
                    this.offsetX = -PlayerController.speed / 2f;
                }
            }*/
        }
        if (PlayerPrefs.GetInt("CameraLock", 0) == 1)
        {
            if(Input.GetButtonDown("UnlockCam"))
            {
                cameraTarget = mouseTarget;
            }else if(Input.GetButtonUp("UnlockCam"))
            {
                cameraTarget = truePlayer;
            }
        }
        else {
            if (Input.GetButtonDown("UnlockCam"))
            {
                cameraTarget = truePlayer;
            }
            else if (Input.GetButtonUp("UnlockCam"))
            {
                cameraTarget = mouseTarget;
            }
        }
    }

    private IEnumerator DelayCamera(float WaitForAmount)
    {
        yield return new WaitForSeconds(WaitForAmount);
        active = true;
        yield break;
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
        if (localPlayer != null && localPlayer.hasAuthority)
        {
            cameraTarget = localPlayer.gameObject.transform.GetChild(5).gameObject;
        }
        //this.enabled = (localPlayer != null);
    }
    public void UpdateCameraTarget()
    {
        if (PlayerPrefs.GetInt("CameraLock", 0) == 0)
        {
            cameraTarget = mouseTarget;
        }
        else if (PlayerPrefs.GetInt("CameraLock", 0) == 1)
        {
            cameraTarget = truePlayer;
        }
        else
        {
            cameraTarget = mouseTarget;
        }
    }
}