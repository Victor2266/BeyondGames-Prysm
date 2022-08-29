using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class WeaponUI : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite meleeSprite;
    public Sprite rangedSprite;

    public GameObject MeleeUI;
    public GameObject RangedUI;
    public GameObject WeaponRedUI;
    public GameObject WeaponOrangeUI;
    public GameObject WeaponYellowUI;
    public GameObject WeaponGreenUI;
    public GameObject WeaponBlueUI;
    public GameObject WeaponIndigoUI;
    public GameObject WeaponVioletUI;
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
    public WeaponController handheld_weapon;

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

        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem == null)
        {
            if (oldItem.equipSlot == EquipmentSlot.melee)
            {
                MeleeUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.ranged)
            {
                RangedUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.red)
            {
                WeaponRedUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.orange)
            {
                WeaponOrangeUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.yellow)
            {
                WeaponYellowUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.green)
            {
                WeaponGreenUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.blue)
            {
                WeaponBlueUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.indigo)
            {
                WeaponIndigoUI.GetComponent<Image>().sprite = emptySprite;
            }
            else if (oldItem.equipSlot == EquipmentSlot.violet)
            {
                WeaponVioletUI.GetComponent<Image>().sprite = emptySprite;
            }
            while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
            {
                playerScript.weapon++;

            }
            if (playerScript.weapon == 7 && !EquipmentManager.instance.isEquipped(playerScript.weapon))//[7]
            {
                playerScript.weapon = -1;
                while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
                {
                    playerScript.weapon++;

                }
            }
        }
        else if(newItem.equipSlot == EquipmentSlot.melee)
        {
            MeleeUI.GetComponent<Image>().sprite = meleeSprite;
            autoChangeWeapons(-1);
        }
        else if (newItem.equipSlot == EquipmentSlot.ranged)
        {
            RangedUI.GetComponent<Image>().sprite = rangedSprite;
            autoChangeWeapons(0);
        }
        else if (newItem.equipSlot == EquipmentSlot.red)
        {
            WeaponRedUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(1);
        }
        else if (newItem.equipSlot == EquipmentSlot.orange)
        {
            WeaponOrangeUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(2);
        }
        else if (newItem.equipSlot == EquipmentSlot.yellow)
        {
            WeaponYellowUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(3);
        }
        else if (newItem.equipSlot == EquipmentSlot.green)
        {
            WeaponGreenUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(4);
        }
        else if (newItem.equipSlot == EquipmentSlot.blue)
        {
            WeaponBlueUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(5);
        }
        else if (newItem.equipSlot == EquipmentSlot.indigo)
        {
            WeaponIndigoUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(6);
        }
        else if (newItem.equipSlot == EquipmentSlot.violet)
        {
            WeaponVioletUI.GetComponent<Image>().sprite = newItem.icon;
            autoChangeWeapons(7);
        }


    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (Mathf.Abs(SliderUI.anchoredPosition.x - (float)TruePosition) > 0.01f)
        {
            posX = Mathf.SmoothDamp(SliderUI.anchoredPosition.x, (float)TruePosition, ref velocity.y, 0.1f);
            SliderUI.anchoredPosition = new Vector2(posX, 0f);
            SliderParentUI.anchoredPosition = new Vector2(posX, 0f);
            //playerScript.ChargeIndicator.SetActive(false);

            manaFillColor.color = SliderColour.color;

        }

        if ((Input.GetKey(KeyCode.Alpha1) ) && EquipmentManager.instance.isEquipped(-1))
        {
            autoChangeWeapons(-1);
        }
        if ((Input.GetKey(KeyCode.Alpha2) ) && EquipmentManager.instance.isEquipped(0))
        {
            autoChangeWeapons(0);
        }
        if ((Input.GetKey(KeyCode.Alpha3) ) && EquipmentManager.instance.isEquipped(1))
        {
            autoChangeWeapons(1);
        }
        if ((Input.GetKey(KeyCode.Alpha4) ) && EquipmentManager.instance.isEquipped(2))
        {
            autoChangeWeapons(2);
        }
        if ((Input.GetKey(KeyCode.Alpha5)) && EquipmentManager.instance.isEquipped(3))
        {
            autoChangeWeapons(3);
        }
        if ((Input.GetKey(KeyCode.Alpha6)) && EquipmentManager.instance.isEquipped(4))
        {
            autoChangeWeapons(4);
        }
        if ((Input.GetKey(KeyCode.Alpha7)) && EquipmentManager.instance.isEquipped(5))
        {
            autoChangeWeapons(5);
        }
        if ((Input.GetKey(KeyCode.Alpha8)) && EquipmentManager.instance.isEquipped(6))
        {
            autoChangeWeapons(6);
        }
        if ((Input.GetKey(KeyCode.Alpha9)) && EquipmentManager.instance.isEquipped(7))
        {
            autoChangeWeapons(7);
        }



        if (Input.GetAxis("Mouse ScrollWheel") > 0.09f) // forward
        {
            if (playerScript.weapon > -1 && playerScript.weapon < 8)//from weapons -1 [0 1234567]
            {
                playerScript.weapon--;
            }
            

            if (playerScript.weapon > -2 && playerScript.weapon <= 7)
            {
                while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon > -1)
                {
                    playerScript.weapon--;
                    
                }
                if (playerScript.weapon == -1 && !EquipmentManager.instance.isEquipped(playerScript.weapon))//[-1]
                {
                    playerScript.weapon = 7;
                    while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon > -1)
                    {
                        playerScript.weapon--;

                    }
                }

                autoChangeWeapons(playerScript.weapon);
                playerManager.SetWeap();
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < -0.09f) // backwards
        {
            if (playerScript.weapon > -2 && playerScript.weapon < 7)//from weapons [-1 0 123456]7
            {
                playerScript.weapon++;
            }
            

            if (playerScript.weapon > -2 && playerScript.weapon <= 7)
            {
                while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
                {
                    playerScript.weapon++;
                    
                }
                if (playerScript.weapon == 7 && !EquipmentManager.instance.isEquipped(playerScript.weapon))//[7]
                {
                    playerScript.weapon = -1;
                    while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
                    {
                        playerScript.weapon++;

                    }
                }
                autoChangeWeapons(playerScript.weapon);
                playerManager.SetWeap();
            }
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
        SliderColour.color = Color.Lerp(SliderColour.color, CurrentColor, 0.1f);
    }

    public void ScaleDown(float r_cooldowntime)
    {
        SliderUI.sizeDelta = new Vector2(30, 0);
        cooldowntime = r_cooldowntime;
    }
    public void flashWhite()
    {
        SliderColour.color = Color.white;
    }

    public void autoChangeWeapons(int weaponNumber)
    {
        if (EquipmentManager.instance.getWeaponType(weaponNumber) == InventoryUI.WeaponTypes.Weapons)//change weapons
        {
            ChangeWeapons(weaponNumber, Mathf.Abs(7 - weaponNumber) * -30, EquipmentManager.instance.getColor(weaponNumber), true);
        }
        else if (EquipmentManager.instance.getWeaponType(weaponNumber) == InventoryUI.WeaponTypes.Spells)
        {
            ChangeWeapons(weaponNumber, Mathf.Abs(7 - weaponNumber) * -30, EquipmentManager.instance.getColor(weaponNumber), false);
        }
    }
    void ChangeWeapons(int weap, int pos, Color col, bool inHand)
    {
        if(weap >= 1)
        {
            playerScript.weapon = weap;
            playerScript.weaponsList[weap - 1] = EquipmentManager.instance.getSpells(weap)[0];
            playerScript.weaponsList[weap + 6] = EquipmentManager.instance.getSpells(weap)[1];
            TruePosition = pos + xAdjustment;
            CurrentColor = col;
            playerScript.ChargeIndicator.GetComponent<ParticleSystem>().startColor = col;
            
            handheld_weapon.onHeldInHand.Invoke(false);
            playerManager.SetWeap();
        }
        else
        {
            playerScript.weapon = weap;
            TruePosition = pos + xAdjustment;
            CurrentColor = col;
            playerScript.ChargeIndicator.GetComponent<ParticleSystem>().startColor = col;
      
            handheld_weapon.onHeldInHand.Invoke(true);
            playerManager.SetWeap();
        }
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