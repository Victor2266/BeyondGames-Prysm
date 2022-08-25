using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int MaxMana = 100; //the mana cap for your mana, upgrades change it

    public int MaxHealth = 100; //health cap, it changes with upgrades

    public float cameraSize = 3f;//camera size, these are public static because they change

    public int Level = 1;

    public int Lives = 1;//number of lives, changes

    public float speed = 3.8f;//max speed, changes (The value for single player is managed in my scene manager)

    public float jumpForce = 4.9f;//how high the player can jump

    public float fallMultiplier = 2.5f;// the gravity on a high jump is greater

    public float lowJumpMultiplier = 2f;//lower gravity for low jump

    public float currentHealth;//current health of player

    public int currentMana;//current mana player has

    public int ManaCost;//cost of the weapon the player is using in mana

    public bool isDead;//if the player is dead

    public bool isDying;//if the player has 0 health and is dying

    public int charges = 1;//how many times the bullet shoots out

    public float timeStamp;//this calculates delta time for the cooldown of weapons
    public float SlideCooldown;

    public float coolDownPeriod;//cooldown val for weapons

    public int weapon;//weapon number

    public float attackVelo;//velocity of weapon

    public bool rapid_fire;

    public bool power_control;

    public float inaccuracy;

    public int[] Chargeable = new int[14];//array keeps track of which charged shots you've unlocked
  
    public bool isClimbing;

    public int HealthBarScalingLength;

    public bool customLocalPlayerCheck;
    public bool lookingLeft;

    public float[] CheckpointPos;

    public PlayerSaveData (PlayerEntity player)
    {
        MaxMana = player.MaxMana;
        MaxHealth = player.MaxHealth;
        cameraSize = player.cameraSize;

        Level = player.Level;
        Lives = player.Lives;
        speed = player.speed;
        jumpForce = player.jumpForce;
        fallMultiplier = player.fallMultiplier;
        lowJumpMultiplier = player.lowJumpMultiplier;
        currentHealth = player.currentHealth;
        currentMana = player.currentMana;
        ManaCost = player.ManaCost;
        isDead = player.isDead;
        isDying = player.isDying;
        charges = player.charges;
        timeStamp = player.timeStamp;
        SlideCooldown = player.SlideCooldown;
        coolDownPeriod = player.coolDownPeriod;
        weapon = player.weapon;
        attackVelo = player.attackVelo;
        rapid_fire = player.rapid_fire;
        power_control = player.power_control;
        inaccuracy = player.inaccuracy;
        isClimbing = player.isClimbing;
        HealthBarScalingLength = player.HealthBarScalingLength;
        customLocalPlayerCheck = player.customLocalPlayerCheck;
        lookingLeft = player.lookingLeft;

        CheckpointPos = new float[3];
        CheckpointPos[0] = player.CheckpointPos.x;
        CheckpointPos[1] = player.CheckpointPos.y;
        CheckpointPos[2] = player.CheckpointPos.z;
    }
}
