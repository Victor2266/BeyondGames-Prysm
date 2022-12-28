using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class PlayerEntity : MonoBehaviour
{
    public int MaxMana = 100; //the mana cap for your mana, upgrades change it

    public int MaxHealth = 100; //health cap, it changes with upgrades

    public float cameraSize = 3f;//camera size, these are public static because they change

    //public int Level = 1;

    public int Souls = 0;//number of lives, changes

    public float speed = 3.8f;//max speed, changes (The value for single player is managed in my scene manager)

    public GameObject bullet;//this game object contains the bullet, to position and impart velocity on it

    public float jumpForce = 4.9f;//how high the player can jump

    public float fallMultiplier = 2.5f;// the gravity on a high jump is greater

    public float lowJumpMultiplier = 2f;//lower gravity for low jump

    public Rigidbody2D rb2d;// ridgidbody for physics
    //public SpriteRenderer SprtRnderer;

    public GameObject Camera;//camera reference 
    public GameObject CameraOrbObj;

    public bool Flinch;//when the skeleton hits the player the player will flinch

    public Text LivesUI;//this is the text number for the lives in the ui
    public Text HealthUIText;
    public Text ManaUIText;

    public Slider health;//this is the health slider in ui

    public float currentHealth;//current health of player

    public Slider mana;//slider for mana in UI

    public int currentMana;//current mana player has

    public int ManaCost;//cost of the weapon the player is using in mana

    public GameObject transition;//the black transition object

    public bool isDead;//if the player is dead

    public bool isDying;//if the player has 0 health and is dying

    public int charges = 1;//how many times the bullet shoots out

    public float timeStamp;//this calculates delta time for the cooldown of weapons
    public float SlideCooldown;

    public float coolDownPeriod;//cooldown val for weapons

    public int weapon;//weapon number

    public GameObject attack;//actual weapon prefab

    public float attackVelo;//velocity of weapon

    public bool rapid_fire;

    public bool power_control;

    public float inaccuracy;

    public GameObject[] weaponsList = new GameObject[14];


    public GameObject ChargeIndicator;//the particle system when you use a charged shot

    public GameObject Orb;
    public GameObject mousePointer = null;

    public OldCameraController OrbPosition;

    public GameObject gasPuff;
    public GameObject bloodPuff;
    public GameObject spawnedEffect;
    public GameObject redFlash;

    public GameObject speedTrail;
    public bool isClimbing;

    public int HealthBarScalingLength;

    public GameObject TextPopUp;

    public RectTransform healthRect;//health slider rect transform
    public RectTransform manaRect;//mana slider rect transform

    public bool customLocalPlayerCheck = true;
    public bool lookingLeft;

    public WeaponUI WeaponUI;

    public GameObject FloorContact;

    public Transform BlackBodyParticles;
    public GameObject Skull;
    public GameObject burp;
    public Rigidbody2D skullRB2D;
    public GameObject deathParticles;
    public GameObject handheldWeapon;

    public RectTransform redHealth;
    public RectTransform whiteMana;

    public ParticleSystem leftBosoter;
    public ParticleSystem rightBooster;
    public AudioSource audioSource;
    public void PlayerEntityUpdate(PlayerSaveData player)
    {
        MaxMana = player.MaxMana;
        MaxHealth = player.MaxHealth;
        cameraSize = player.cameraSize;

        //Level = player.Level;
        Souls = player.Souls;
        speed = player.speed;
        jumpForce = player.jumpForce;
        fallMultiplier = player.fallMultiplier;
        lowJumpMultiplier = player.lowJumpMultiplier;

        currentHealth = player.MaxHealth;//sets to max value not current
        currentMana = player.MaxMana;

        ManaCost = player.ManaCost;
        isDead = false;
        isDying = false;
        charges = player.charges;
        timeStamp = 0f;
        SlideCooldown = 0f;//don't want to spawn with cooldowns
        coolDownPeriod = 0f;
        weapon = player.weapon;
        attackVelo = player.attackVelo;
        rapid_fire = player.rapid_fire;
        power_control = player.power_control;
        inaccuracy = player.inaccuracy;
        isClimbing = player.isClimbing;
        HealthBarScalingLength = player.HealthBarScalingLength;
        customLocalPlayerCheck = player.customLocalPlayerCheck;
        lookingLeft = player.lookingLeft;

        //CheckpointPos = new Vector3(player.CheckpointPos[0], player.CheckpointPos[1], player.CheckpointPos[2]);
    }

    public void setHealth(float val)
    {
        currentHealth = val;

        UpdateHealth();
    }
    public void setMana(int val)
    {
        currentMana = val;

        UpdateMana();
    }
    public void setHealthAndMana(float health, int mana)
    {
        LeanTween.cancel(gameObject);
        currentHealth = health;
        currentMana = mana;

        UpdateHealth();
        UpdateMana();
    }

    public void UpdateHealth()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        LivesUI.text = "souls. " + Souls.ToString();

        HealthUIText.text = currentHealth.ToString() + "/" + MaxHealth.ToString();
        //playerEntity.HealthUIText.rectTransform.position = new Vector3(playerEntity.healthRect.sizeDelta.x + 2, playerEntity.HealthUIText.rectTransform.position.y, playerEntity.HealthUIText.rectTransform.position.z);
        
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.value(gameObject, health.value, currentHealth, 0.1f).setOnUpdate((float val) => { health.value = val; }));
        sequence.append(LeanTween.scale(redHealth, new Vector3(currentHealth / MaxHealth, 1f, 1f), 1f));
    }
    public void UpdateMana()
    {
        ManaUIText.text = currentMana.ToString() + "/" + MaxMana.ToString();
        //playerEntity.ManaUIText.rectTransform.position = new Vector3(playerEntity.manaRect.sizeDelta.x + 2, playerEntity.ManaUIText.rectTransform.position.y, playerEntity.ManaUIText.rectTransform.position.z);

        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.value(gameObject, mana.value, currentMana, 0.1f).setOnUpdate((float val) => { mana.value = val; }));

        sequence.append(LeanTween.scale(whiteMana, new Vector3((float)currentMana / MaxMana, 1f, 1f), 1f));
    }
}
