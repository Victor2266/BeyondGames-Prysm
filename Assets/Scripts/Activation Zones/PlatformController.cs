using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlatformController : MonoBehaviour
{
    public GameObject Player;
    private float delayTime;


    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player = collision.gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            delayTime = 0.1f;
        }
        if (Input.GetAxisRaw("Vertical") < 0f)
        {
            if (delayTime <= 0)
            {
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.GetComponent<CapsuleCollider2D>(), true);
            }
            else
            {
                delayTime -= Time.deltaTime;
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0f)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.GetComponent<CapsuleCollider2D>(), false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.GetComponent<CapsuleCollider2D>(), false);
        }
        if (Input.GetButtonUp("Vertical"))
        {
            StartCoroutine(DelayedActivation());
        }
    }
    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(0.15f);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.GetComponent<CapsuleCollider2D>(), false);
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
            Player = localPlayer.gameObject;
        }
        //this.enabled = (localPlayer != null);
    }
}
