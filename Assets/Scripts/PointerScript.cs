using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PointerScript : MonoBehaviour
{

    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;
    }


    private void Update()
    {
        this.xDiff = this.boss.transform.position.x - base.transform.position.x;
        this.yDiff = this.boss.transform.position.y - base.transform.position.y;
        this.XAngle = 57.29578f * Mathf.Atan(this.yDiff / this.xDiff);
        if (isEyeBall)
        {
            if (noFlip == false)
            {
                if (this.xDiff > 0f)
                {
                    base.transform.eulerAngles = new Vector3(0f, 0f, offset + XAngle);
                }
                else if (this.xDiff < 0f)
                {
                    base.transform.eulerAngles = new Vector3(0f, 0f, -offset + XAngle);
                }
            }
            else
            {
                if (this.xDiff > 0f)
                {
                    base.transform.eulerAngles = new Vector3(0f, 0f, offset + Mathf.Abs(XAngle));
                }
                else if (this.xDiff < 0f)
                {
                    base.transform.eulerAngles = new Vector3(0f, 0f, offset + 180f - Mathf.Abs(XAngle));
                }
            }
            

        }
        else if (this.xDiff > 0f)
        {
            base.transform.eulerAngles = new Vector3(360f - this.XAngle, 90f, 0f);
        }
        else if (this.xDiff < 0f)
        {
            base.transform.eulerAngles = new Vector3(180f - this.XAngle, 90f, 0f);
        }
        
        
        
        if (!this.boss.activeSelf || boss == null)
        {
            this.boss.transform.position = new Vector2(this.xDest, this.yDest);
        }
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
        if (localPlayer != null && IsLookingAtPlayer)
        {
            boss = localPlayer.gameObject;
        }
        //this.enabled = (localPlayer != null);
    }

    [SerializeField] private bool IsLookingAtPlayer = false;

    public float xDest;
    public float yDest;
    public bool noFlip;

    private float xDiff;

    private float yDiff;

    private float XAngle;
    public float offset;
    public GameObject boss;
    public bool isEyeBall;
}