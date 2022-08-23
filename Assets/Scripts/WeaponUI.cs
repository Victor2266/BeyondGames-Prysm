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

    [SerializeField]
    private int xAdjustment;
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
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem == null)
        {
            if (oldItem.equipSlot == EquipmentSlot.melee)
            {

            }
            else if (oldItem.equipSlot == EquipmentSlot.ranged)
            {

            }
            else if (oldItem.equipSlot == EquipmentSlot.red)
            {
                WeaponRedUI.GetComponent<Image>().sprite = null;
            }
            else if (oldItem.equipSlot == EquipmentSlot.orange)
            {
                WeaponOrangeUI.GetComponent<Image>().sprite = null;
            }
            else if (oldItem.equipSlot == EquipmentSlot.yellow)
            {
                WeaponYellowUI.GetComponent<Image>().sprite = null;
            }
            else if (oldItem.equipSlot == EquipmentSlot.green)
            {
                WeaponGreenUI.GetComponent<Image>().sprite = null;
            }
            else if (oldItem.equipSlot == EquipmentSlot.blue)
            {
                WeaponBlueUI.GetComponent<Image>().sprite = null;
            }
            else if (oldItem.equipSlot == EquipmentSlot.indigo)
            {
                WeaponIndigoUI.GetComponent<Image>().sprite = null;
            }
            else if (oldItem.equipSlot == EquipmentSlot.violet)
            {
                WeaponVioletUI.GetComponent<Image>().sprite = null;
            }
        }
        else if(newItem.equipSlot == EquipmentSlot.melee)
        {

        }
        else if (newItem.equipSlot == EquipmentSlot.ranged)
        {

        }
        else if (newItem.equipSlot == EquipmentSlot.red)
        {
            WeaponRedUI.GetComponent<Image>().sprite = newItem.icon;
        }
        else if (newItem.equipSlot == EquipmentSlot.orange)
        {
            WeaponOrangeUI.GetComponent<Image>().sprite = newItem.icon;
        }
        else if (newItem.equipSlot == EquipmentSlot.yellow)
        {
            WeaponYellowUI.GetComponent<Image>().sprite = newItem.icon;
        }
        else if (newItem.equipSlot == EquipmentSlot.green)
        {
            WeaponGreenUI.GetComponent<Image>().sprite = newItem.icon;
        }
        else if (newItem.equipSlot == EquipmentSlot.blue)
        {
            WeaponBlueUI.GetComponent<Image>().sprite = newItem.icon;
        }
        else if (newItem.equipSlot == EquipmentSlot.indigo)
        {
            WeaponIndigoUI.GetComponent<Image>().sprite = newItem.icon;
        }
        else if (newItem.equipSlot == EquipmentSlot.violet)
        {
            WeaponVioletUI.GetComponent<Image>().sprite = newItem.icon;
        }


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
           
            playerScript.Chargeable = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        }

        if ((Input.GetKey(KeyCode.Alpha1) || playerScript.weapon == 1) && EquipmentManager.instance.isEquipped(1))
        {   
            ChangeWeapons(1, -180, new Color(1f, 0f, 0f, 1f));
        }
        if ((Input.GetKey(KeyCode.Alpha2) || playerScript.weapon == 2) && EquipmentManager.instance.isEquipped(2))
        {
            ChangeWeapons(2, -150, new Color(1f, 0.5f, 0f, 0.75f));
        }
        if ((Input.GetKey(KeyCode.Alpha3) || playerScript.weapon == 3) && EquipmentManager.instance.isEquipped(3))
        {
            ChangeWeapons(3, -120, new Color(1f, 1f, 0f, 0.75f));
        }
        if ((Input.GetKey(KeyCode.Alpha4) || playerScript.weapon == 4) && EquipmentManager.instance.isEquipped(4))
        {
            ChangeWeapons(4, -90, new Color(0f, 1f, 0f, 0.75f));
        }
        if ((Input.GetKey(KeyCode.Alpha5) || playerScript.weapon == 5) && EquipmentManager.instance.isEquipped(5))
        {
            ChangeWeapons(5, -60, new Color(0.15f, 0.3f, 1f, 1f));
        }
        if ((Input.GetKey(KeyCode.Alpha6) || playerScript.weapon == 6) && EquipmentManager.instance.isEquipped(6))
        {
            ChangeWeapons(6, -30, new Color(0.5f, 0.25f, 1f, 1f));
        }
        if ((Input.GetKey(KeyCode.Alpha7) || playerScript.weapon == 7) && EquipmentManager.instance.isEquipped(7))
        {
            ChangeWeapons(7, 0, new Color(1f, 0.2f, 1f, 1f));
        
        }



        if (Input.GetAxis("Mouse ScrollWheel") > 0.09f) // forward
        {
            if (playerScript.weapon > 1 && playerScript.weapon < 8)
            {
                playerScript.weapon--;
            }
            

            if (playerScript.weapon > 0 && playerScript.weapon <= 7)
            {
                while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon > 1)
                {
                    playerScript.weapon--;
                    
                }
                if (playerScript.weapon == 1 && !EquipmentManager.instance.isEquipped(playerScript.weapon))
                {
                    playerScript.weapon = 7;
                    while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon > 1)
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
                while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
                {
                    playerScript.weapon++;
                    
                }
                if (playerScript.weapon == 7 && !EquipmentManager.instance.isEquipped(playerScript.weapon))
                {
                    playerScript.weapon = 1;
                    while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
                    {
                        playerScript.weapon++;

                    }
                }
            }


            playerManager.SetWeap();
        }


        if (playerScript.weapon > 7)
        {
            sliderYSize = Mathf.SmoothDamp(SliderUI.sizeDelta.y, 96, ref sizeRefVelo, cooldowntime / 2f);
            sliderParentYSize = Mathf.SmoothDamp(SliderParentUI.sizeDelta.y, 96, ref ParentsizeRefVelo, cooldowntime / 16f);

            SliderUI.sizeDelta = new Vector2(30, sliderYSize);
            SliderParentUI.sizeDelta = new Vector2(30, sliderParentYSize);
        }
        else if (playerScript.weapon < 8)
        {
            sliderYSize = Mathf.SmoothDamp(SliderUI.sizeDelta.y, 64, ref sizeRefVelo, cooldowntime / 2f);
            sliderParentYSize = Mathf.SmoothDamp(SliderParentUI.sizeDelta.y, 64, ref ParentsizeRefVelo, cooldowntime /16f);

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
        playerScript.weaponsList[weap - 1] = EquipmentManager.instance.getSpells(weap)[0];
        playerScript.weaponsList[weap + 6] = EquipmentManager.instance.getSpells(weap)[1];
        TruePosition = pos + xAdjustment;
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