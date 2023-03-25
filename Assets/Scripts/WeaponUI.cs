using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    public RectTransform InstaSliderUI;
    private RawImage SliderColour;
    public Vector2 velocity;
    public int TruePosition;
    private float posX;
    public GameObject Player;
    public PlayerInput playerInput;
    
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

    private bool unequipAll = false;
    [SerializeField]
    private int xAdjustment;

    private float scrollVal;

    private float selectWeapon;

    public bool toggleWeapSpecial = false;
    /*
    private void Awake() //NETWORKING AWAKE
    {
        PlayerUpdated(ClientScene.localPlayer);
        LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;
    }*/

    private void Awake()
    {
        //EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }
    private void OnEnable()
    {
        //EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        manaFillColor = FillObj.GetComponent<Image>();
        SliderUI = Slider.GetComponent<RectTransform>();
        SliderParentUI = SliderParent.GetComponent<RectTransform>();
        SliderColour = Slider.GetComponent<RawImage>();

        playerScript = Player.GetComponent<PlayerEntity>();
        playerManager = Player.GetComponent<PlayerManager>();

        //OrbObject = GameObject.FindWithTag("OrbControler");
        TruePosition = 0;
        CurrentColor = new Color(1f, 1f, 0f, 0f);
    }
    private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        unequipAll = false;
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

            while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)//[-1 0 1 2 3 4 5 6] 7
            {
                playerScript.weapon++;
            }
            if (playerScript.weapon == 7 && !EquipmentManager.instance.isEquipped(playerScript.weapon)) //[7]
            {
                playerScript.weapon = -1;
                Debug.Log("go to weapon -1");
                while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)//[-1 0 1 2 3 4 5 6] 7
                {
                    playerScript.weapon++;
                }
                if (playerScript.weapon == 7)//nothing is equipped because we checked every slot
                {
                    playerScript.weapon = -1;
                    unequipAll = true;
                }
            }
            if (!unequipAll)
            {
                autoChangeWeapons(playerScript.weapon);
                playerManager.SetWeap();
            }
            else
            {
                autoChangeWeapons(15);
                playerManager.SetWeap();
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

    private float DistToFinalPos;
    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        DistToFinalPos = Mathf.Abs(SliderUI.anchoredPosition.x - (float)TruePosition);

        if (DistToFinalPos > 0.01f)
        {
            posX = Mathf.SmoothDamp(SliderUI.anchoredPosition.x, (float)TruePosition, ref velocity.y, 0.1f);
            InstaSliderUI.anchoredPosition = new Vector2(TruePosition, 0f);
            SliderUI.anchoredPosition = new Vector2(posX, 0f);
            SliderParentUI.anchoredPosition = new Vector2(posX, 0f);
            //playerScript.ChargeIndicator.SetActive(false);

            manaFillColor.color = SliderColour.color;

        }

        if (playerInput.actions["Select Weapon"].IsPressed())
        {
            Debug.Log(playerInput.actions["Select Weapon"].ReadValue<float>());
            selectWeapon = playerInput.actions["Select Weapon"].ReadValue<float>();

            if (selectWeapon > playerScript.weapon + 2)
            {
                StretchRight();
            }
            else if (selectWeapon < playerScript.weapon + 2)
            {
                StretchLeft();
            }
        }
        else
        {
            selectWeapon = 0;
        }

        if ((selectWeapon == 1) && EquipmentManager.instance.isEquipped(-1))
        {
            autoChangeWeapons(-1);
        }
        else if ((selectWeapon == 2) && EquipmentManager.instance.isEquipped(0))
        {
            autoChangeWeapons(0);
        }
        else if ((selectWeapon == 3) && EquipmentManager.instance.isEquipped(1))
        {
            autoChangeWeapons(1);
        }
        else if ((selectWeapon == 4) && EquipmentManager.instance.isEquipped(2))
        {
            autoChangeWeapons(2);
        }
        else if ((selectWeapon == 5) && EquipmentManager.instance.isEquipped(3))
        {
            autoChangeWeapons(3);
        }
        else if ((selectWeapon == 6) && EquipmentManager.instance.isEquipped(4))
        {
            autoChangeWeapons(4);
        }
        else if ((selectWeapon == 7) && EquipmentManager.instance.isEquipped(5))
        {
            autoChangeWeapons(5);
        }
        else if ((selectWeapon == 8) && EquipmentManager.instance.isEquipped(6))
        {
            autoChangeWeapons(6);
        }
        else if ((selectWeapon == 9) && EquipmentManager.instance.isEquipped(7))
        {
            autoChangeWeapons(7);
        }


        if (!unequipAll && playerScript.charges == 1)//if wearing equipment and not holding an active magic spell
        {
            if (playerInput.actions["Change Weapon"].WasPerformedThisFrame())
            {
                scrollVal = playerInput.actions["Change Weapon"].ReadValue<float>();
                if (scrollVal != 0)
                {
                    UnChargeWeapon();
                }
            }

            if (scrollVal > 0.09f) // forward
            {
                if (playerScript.weapon > -1 && playerScript.weapon < 8)//from weapons -1 [0 1234567]
                {
                    playerScript.weapon--;

                    StretchLeft();
                }


                if (playerScript.weapon > -2 && playerScript.weapon <= 7)
                {
                    while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon > -1)
                    {
                        playerScript.weapon--;

                    }
                    if (playerScript.weapon == -1 && !EquipmentManager.instance.isEquipped(playerScript.weapon))//[-1]
                    {
                        playerScript.weapon = 0;
                        while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon > -1 && playerScript.weapon < 7)
                        {
                            playerScript.weapon++;

                        }
                    }

                    autoChangeWeapons(playerScript.weapon);
                    playerManager.SetWeap();
                }
            }
            else if (scrollVal < -0.09f) // backwards
            {
                if (playerScript.weapon > -2 && playerScript.weapon < 7)//from weapons [-1 0 123456]7
                {
                    playerScript.weapon++;

                    StretchRight();
                }


                if (playerScript.weapon > -2 && playerScript.weapon <= 7)
                {
                    while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7)
                    {
                        playerScript.weapon++;

                    }
                    if (playerScript.weapon == 7 && !EquipmentManager.instance.isEquipped(playerScript.weapon))//[7]
                    {
                        playerScript.weapon = 6;
                        while (!EquipmentManager.instance.isEquipped(playerScript.weapon) && playerScript.weapon < 7 && playerScript.weapon > -1)
                        {
                            playerScript.weapon--;

                        }
                    }
                    autoChangeWeapons(playerScript.weapon);
                    playerManager.SetWeap();
                }
            }
            
            scrollVal = 0f;
        }
        


        if (playerScript.weapon > 7 || toggleWeapSpecial)
        {
            sliderYSize = Mathf.SmoothDamp(SliderUI.sizeDelta.y, 96, ref sizeRefVelo, cooldowntime / 2f);
            sliderParentYSize = Mathf.SmoothDamp(SliderParentUI.sizeDelta.y, 96, ref ParentsizeRefVelo, cooldowntime / 16f);

            SliderUI.sizeDelta = new Vector2(30 + DistToFinalPos /2f, sliderYSize);
            SliderParentUI.sizeDelta = new Vector2(30 + DistToFinalPos / 2f, sliderParentYSize);
        }
        else if (playerScript.weapon < 8)
        {
            sliderYSize = Mathf.SmoothDamp(SliderUI.sizeDelta.y, 64, ref sizeRefVelo, cooldowntime / 2f);
            sliderParentYSize = Mathf.SmoothDamp(SliderParentUI.sizeDelta.y, 64, ref ParentsizeRefVelo, cooldowntime /16f);

            SliderUI.sizeDelta = new Vector2(30 + DistToFinalPos / 2f, sliderYSize);
            SliderParentUI.sizeDelta = new Vector2(30 + DistToFinalPos / 2f, sliderParentYSize);
        }
        SliderColour.color = Color.Lerp(SliderColour.color, CurrentColor, 0.1f);
  
    }
    public void UnChargeWeapon()
    {
        if (playerScript.weapon >= 8)
        {
            playerScript.ChargeIndicator.SetActive(false);
            playerScript.weapon -= 7;
            //Debug.Log("chrge down");
        }
        else if (playerScript.weapon < 1)
        {
            toggleWeapSpecial = false;
            Debug.Log("WEAPON TOGGLE OFF");
        }
        playerManager.SetWeap();
    }
    public void ScaleDown(float r_cooldowntime)
    {
        SliderUI.sizeDelta = new Vector2(30, 0);
        cooldowntime = r_cooldowntime;
    }
    public void StretchRight()
    {
        SliderUI.pivot = new Vector2(0, 1f);
        SliderParentUI.pivot = new Vector2(0, 1f);
        InstaSliderUI.pivot = new Vector2(0, 1f);
        if (xAdjustment == -13)
        {
            SliderUI.anchoredPosition = new Vector2(SliderUI.anchoredPosition.x - 30, SliderUI.anchoredPosition.y);
            SliderParentUI.anchoredPosition = new Vector2(SliderParentUI.anchoredPosition.x - 30, SliderParentUI.anchoredPosition.y);
            InstaSliderUI.anchoredPosition = new Vector2(InstaSliderUI.anchoredPosition.x - 30, InstaSliderUI.anchoredPosition.y);
        }
        xAdjustment = -43;

    }
    public void StretchLeft()
    {
        SliderUI.pivot = new Vector2(1f, 1f);
        SliderParentUI.pivot = new Vector2(1f, 1f);
        InstaSliderUI.pivot = new Vector2(1f, 1f);
        if (xAdjustment == -43)
        {
            SliderUI.anchoredPosition = new Vector2(SliderUI.anchoredPosition.x + 30, SliderUI.anchoredPosition.y);
            SliderParentUI.anchoredPosition = new Vector2(SliderParentUI.anchoredPosition.x + 30, SliderParentUI.anchoredPosition.y);
            InstaSliderUI.anchoredPosition = new Vector2(InstaSliderUI.anchoredPosition.x + 30, InstaSliderUI.anchoredPosition.y);
        }
        xAdjustment = -13;
    }
    public void flashWhite()
    {
        SliderColour.color = Color.white;
    }

    public void autoChangeWeapons(int weaponNumber)
    {
        if(weaponNumber == 15)//15 means nothing is equipped
        {
            playerScript.weapon = -1;//setting this to -1 so no magic is used
            ChangeWeapons(-1, 50, Color.black, false);
        }

        else if (EquipmentManager.instance.getWeaponType(weaponNumber) == InventoryUI.WeaponTypes.Weapons)//change weapons
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
      
            handheld_weapon.onHeldInHand.Invoke(inHand);
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