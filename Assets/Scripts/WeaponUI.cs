using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class WeaponUI : MonoBehaviour
{

    public GameObject WeaponRedUI;
    public GameObject WeaponOrangeUI;
    public GameObject WeaponYellowUI;
    public GameObject WeaponGreenUI;
    public GameObject WeaponBlueUI;
    public GameObject WeaponIndigoUI;
    public GameObject WeaponVioletUI;
    public GameObject[] weaponList;
    public GameObject Slider;
    public GameObject SliderParent;
    private RectTransform SliderParentUI;
    private RectTransform SliderUI;
    private RawImage SliderColour;
    public Vector2 velocity;
    public int TruePosition;
    private float posX;
    public GameObject Player;
    
    private PlayerEntity playerScript;
    private PlayerManager playerManager;

    public GameObject FillObj;
    private Image manaFillColor;
    public Color CurrentColor;
    private float sizeRefVelo;
    private float ParentsizeRefVelo;
    private float sliderYSize;
    private float sliderParentYSize;

    public GameObject OrbObject;
    public bool DoneScaling;
    public float cooldowntime;

    /*
    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;
    }*/

    private void Start()
    {
        manaFillColor = FillObj.GetComponent<Image>();
        SliderUI = Slider.GetComponent<RectTransform>();
        SliderParentUI = SliderParent.GetComponent<RectTransform>();
        SliderColour = Slider.GetComponent<RawImage>();

        playerScript = Player.GetComponent<PlayerEntity>();
        playerManager = Player.GetComponent<PlayerManager>();

        //OrbObject = GameObject.FindWithTag("OrbControler");
        TruePosition = 0;
        CurrentColor = new Color(1f, 1f, 0f, 0f);

        weaponList = new GameObject[] { WeaponRedUI, WeaponOrangeUI, WeaponYellowUI, WeaponGreenUI, WeaponBlueUI, WeaponIndigoUI, WeaponVioletUI };
    }

    private void Update()
    {
        if (Mathf.Abs(SliderUI.anchoredPosition.x - (float)TruePosition) > 0.01f)
        {
            posX = Mathf.SmoothDamp(SliderUI.anchoredPosition.x, (float)TruePosition, ref velocity.y, 0.2f);
            SliderColour.color = Color.Lerp(SliderColour.color, CurrentColor, 0.1f);
            SliderUI.anchoredPosition = new Vector2(posX, 0f);
            SliderParentUI.anchoredPosition = new Vector2(posX, 0f);
            //playerScript.ChargeIndicator.SetActive(false);

            manaFillColor.color = SliderColour.color;

        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (GameObject weapo in weaponList){
                weapo.SetActive(true);
            }
            playerScript.Chargeable = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        }

        if (Input.GetKey(KeyCode.Alpha1) || playerScript.weapon == 1)
        {   
            TruePosition = -180;
            CurrentColor = new Color(1f, 0f, 0f, 1f);


            playerManager.SetWeap();
        }
        if ((Input.GetKey(KeyCode.Alpha2) || playerScript.weapon == 2) && WeaponOrangeUI.activeSelf == true)
        {
            ChangeWeapons(2, -150, new Color(1f, 0.5f, 0f, 0.75f));
        }
        if ((Input.GetKey(KeyCode.Alpha3) || playerScript.weapon == 3) && WeaponYellowUI.activeSelf == true)
        {
            ChangeWeapons(3, -120, new Color(1f, 1f, 0f, 0.75f));
        }
        if ((Input.GetKey(KeyCode.Alpha4) || playerScript.weapon == 4) && WeaponGreenUI.activeSelf == true)
        {
            ChangeWeapons(4, -90, new Color(0f, 1f, 0f, 0.75f));
        }
        if ((Input.GetKey(KeyCode.Alpha5) || playerScript.weapon == 5) && WeaponGreenUI.activeSelf == true)
        {
            ChangeWeapons(5, -60, new Color(0.15f, 0.3f, 1f, 1f));
        }
        if ((Input.GetKey(KeyCode.Alpha6) || playerScript.weapon == 6) && WeaponIndigoUI.activeSelf == true)
        {
            ChangeWeapons(6, -30, new Color(0.5f, 0.25f, 1f, 1f));
        }
        if (Input.GetKey(KeyCode.Alpha7) || playerScript.weapon == 7)
        {
            TruePosition = 0;
            CurrentColor = new Color(1f, 0.2f, 1f, 255f);



            playerManager.SetWeap();
        }



        if (Input.GetAxis("Mouse ScrollWheel") > 0.09f) // forward
        {
            if (playerScript.weapon > 1 && playerScript.weapon < 8)
            {
                playerScript.weapon--;
            }
            

            if (playerScript.weapon > 0 && playerScript.weapon <= 7)
            {
                while (weaponList[playerScript.weapon - 1].activeSelf == false && playerScript.weapon > 1)
                {
                    playerScript.weapon--;
                    
                }
                if (playerScript.weapon == 1 && weaponList[playerScript.weapon - 1].activeSelf == false)
                {
                    playerScript.weapon = 7;
                    while (weaponList[playerScript.weapon - 1].activeSelf == false && playerScript.weapon > 1)
                    {
                        playerScript.weapon--;

                    }
                }
            }


            playerManager.SetWeap();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < -0.09f) // backwards
        {
            if (playerScript.weapon > 0 && playerScript.weapon < 7)
            {
                playerScript.weapon++;
            }
            

            if (playerScript.weapon > 0 && playerScript.weapon <= 7)
            {
                while (weaponList[playerScript.weapon - 1].activeSelf == false && playerScript.weapon < 7)
                {
                    playerScript.weapon++;
                    
                }
                if (playerScript.weapon == 7 && weaponList[playerScript.weapon - 1].activeSelf == false)
                {
                    playerScript.weapon = 1;
                    while (weaponList[playerScript.weapon - 1].activeSelf == false && playerScript.weapon < 7)
                    {
                        playerScript.weapon++;

                    }
                }
            }


            playerManager.SetWeap();
        }


        if (playerScript.weapon > 7)
        {
            sliderYSize = Mathf.SmoothDamp(SliderUI.sizeDelta.y, 90, ref sizeRefVelo, cooldowntime);
            sliderParentYSize = Mathf.SmoothDamp(SliderParentUI.sizeDelta.y, 90, ref ParentsizeRefVelo, cooldowntime / 10f);

            SliderUI.sizeDelta = new Vector2(30, sliderYSize);
            SliderParentUI.sizeDelta = new Vector2(30, sliderParentYSize);
        }
        else if (playerScript.weapon < 8)
        {
            sliderYSize = Mathf.SmoothDamp(SliderUI.sizeDelta.y, 60, ref sizeRefVelo, cooldowntime);
            sliderParentYSize = Mathf.SmoothDamp(SliderParentUI.sizeDelta.y, 60, ref ParentsizeRefVelo, cooldowntime /10f);

            SliderUI.sizeDelta = new Vector2(30, sliderYSize);
            SliderParentUI.sizeDelta = new Vector2(30, sliderParentYSize);
        }
    }

    public void ScaleDown(float r_cooldowntime)
    {
        SliderUI.sizeDelta = new Vector2(30, 0);
        cooldowntime = r_cooldowntime;
    }

    void ChangeWeapons(int weap, int pos, Color col)
    {
        playerScript.weapon = weap;
        TruePosition = pos;
        CurrentColor = new Vector4(col.r, col.g, col.b, 1f);
        OrbObject.GetComponent<SpriteRenderer>().color = col;
        playerScript.ChargeIndicator.GetComponent<ParticleSystem>().startColor = col;
        playerManager.SetWeap();
    }

    /// <summary>
    /// NETWORKING BELOW
    /// </summary>
    /*
    private void OnDestroy()
    {
        LocalPlayerAnnouncer.OnLocalPlayerUpdated -= PlayerUpdated;
    }
    private void PlayerUpdated(NetworkIdentity localPlayer)
    {
        if (localPlayer != null)
        {
            Player = localPlayer.gameObject;
            OrbObject = localPlayer.gameObject.transform.GetChild(3).gameObject;
            playerScript = Player.GetComponent<PlayerController>();
        }
        //this.enabled = (localPlayer != null);
    }
    */
}