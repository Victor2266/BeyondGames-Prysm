using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEntity : MonoBehaviour
{
    public int MaxMana = 100; //the mana cap for your mana, upgrades change it

    public int MaxHealth = 100; //health cap, it changes with upgrades

    public float cameraSize = 3f;//camera size, these are public static because they change

    public int Level = 1;

    public int Lives = 1;//number of lives, changes

    public float speed = 3.8f;//max speed, changes (The value for single player is managed in my scene manager)

    public GameObject bullet;//this game object contains the bullet, to position and impart velocity on it

    public float jumpForce = 4.9f;//how high the player can jump

    public float fallMultiplier = 2.5f;// the gravity on a high jump is greater

    public float lowJumpMultiplier = 2f;//lower gravity for low jump

    public Rigidbody2D rb2d;// ridgidbody for physics
    public SpriteRenderer SprtRnderer;
    public Animator anim;//animation controller

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

    public int[] Chargeable = new int[14];//array keeps track of which charged shots you've unlocked
    public GameObject Orb;
    public GameObject mousePointer = null;

    public CameraController OrbPosition;

    public GameObject gasPuff;
    public GameObject bloodPuff;
    public GameObject spawnedEffect;
    public GameObject redFlash;

    public GameObject cape;
    public GameObject speedTrail;
    public bool isClimbing;

    public int HealthBarScalingLength;

    public GameObject TextPopUp;

    public RectTransform healthRect;//health slider rect transform
    public RectTransform manaRect;//mana slider rect transform

    public bool customLocalPlayerCheck = true;
    public bool lookingLeft;

    public WeaponUI WeaponUI;

    public Vector3 CheckpointPos;
}
