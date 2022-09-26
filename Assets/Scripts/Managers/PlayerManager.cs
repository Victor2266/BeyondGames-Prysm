﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerEntity playerEntity;

    public void ReStart()
    {
        Start();
    }
    // Start is called before the first frame update
    private void Start()
    {
        SaveSystem.LoadPlayerEntity(playerEntity);
        if (!playerEntity.customLocalPlayerCheck)
            return;
        playerEntity.Camera = GameObject.FindGameObjectWithTag("MainCamera");
        playerEntity.CameraOrbObj = GameObject.FindGameObjectWithTag("Mouse");
        try
        {
            playerEntity.LivesUI = GameObject.FindGameObjectWithTag("LivesUI").GetComponent<Text>();
            playerEntity.HealthUIText = GameObject.FindGameObjectWithTag("Health_Text").GetComponent<Text>();
            playerEntity.ManaUIText = GameObject.FindGameObjectWithTag("Mana_Text").GetComponent<Text>();
            playerEntity.health = GameObject.FindGameObjectWithTag("Health_slider").GetComponent<Slider>();
            playerEntity.mana = GameObject.FindGameObjectWithTag("Mana_slider").GetComponent<Slider>();

            playerEntity.redFlash = GameObject.FindGameObjectWithTag("redFlash");
            if (playerEntity.redFlash.activeSelf)
            {
                playerEntity.redFlash.SetActive(false);
            }

            playerEntity.WeaponUI = GameObject.FindGameObjectWithTag("WeaponUI").GetComponent<WeaponUI>();

            playerEntity.healthRect = playerEntity.health.GetComponent<RectTransform>();
            playerEntity.manaRect = playerEntity.mana.GetComponent<RectTransform>();
        }
        catch (System.Exception e)
        {
            print("missing something here");
        }
        if (playerEntity.mousePointer == null)
        {
            playerEntity.mousePointer = GameObject.FindGameObjectWithTag("Mouse");
        }

        playerEntity.OrbPosition = playerEntity.Orb.GetComponent<OldCameraController>();

        SetWeap();
        playerEntity.ManaCost = 0;
        playerEntity.rb2d = GetComponent<Rigidbody2D>();
        //playerEntity.SprtRnderer = GetComponent<SpriteRenderer>();
        
        playerEntity.Camera.GetComponent<Camera>().orthographicSize = playerEntity.cameraSize;

        //playerEntity.currentHealth = MySceneManager.StartingHealth;
        //playerEntity.currentMana = MySceneManager.StartingMana;

        playerEntity.HealthBarScalingLength = Screen.width / 2;

        if(playerEntity.health != null)
        {
            if (playerEntity.health.maxValue < playerEntity.MaxHealth)
            {
                if (playerEntity.MaxHealth < playerEntity.HealthBarScalingLength)
                {
                    playerEntity.healthRect.sizeDelta = new Vector2((float)playerEntity.MaxHealth, playerEntity.healthRect.sizeDelta.y);
                }
                else
                {
                    playerEntity.healthRect.sizeDelta = new Vector2((float)playerEntity.HealthBarScalingLength, playerEntity.healthRect.sizeDelta.y);
                }
                playerEntity.health.maxValue = (float)playerEntity.MaxHealth;
            }

            if (playerEntity.mana.maxValue < playerEntity.MaxMana)
            {
                if (playerEntity.MaxMana < playerEntity.HealthBarScalingLength)
                {
                    playerEntity.manaRect.sizeDelta = new Vector2((float)playerEntity.MaxMana, playerEntity.manaRect.sizeDelta.y);
                }
                else
                {
                    playerEntity.manaRect.sizeDelta = new Vector2((float)playerEntity.HealthBarScalingLength, playerEntity.manaRect.sizeDelta.y);
                }

                playerEntity.mana.maxValue = (int)playerEntity.MaxMana;
            }
        }

        
    }

    public bool grounded;
    private void Update()
    {
        grounded = IsGrounded();
        if (grounded && playerEntity.FloorContact.activeSelf == false)
        {
            playerEntity.FloorContact.SetActive(true);
        }
        else if (!grounded && playerEntity.FloorContact.activeSelf == true)
        {
            playerEntity.FloorContact.SetActive(false);
        }
        if (!playerEntity.isDying)
        {
            TiltCharacter();
        }
    }

    public void SetWeap()//optimize this later to only run when pressed
    {

        if (playerEntity.weapon >= 1 && playerEntity.weapon <= 14)
        {
            playerEntity.attack = playerEntity.weaponsList[playerEntity.weapon - 1];

            playerEntity.attackVelo = playerEntity.weaponsList[playerEntity.weapon - 1].GetComponent<projectileController>().attackVelo;
            playerEntity.ManaCost = playerEntity.weaponsList[playerEntity.weapon - 1].GetComponent<projectileController>().ManaCost;
            playerEntity.coolDownPeriod = playerEntity.weaponsList[playerEntity.weapon - 1].GetComponent<projectileController>().coolDownPeriod;
            playerEntity.rapid_fire = playerEntity.weaponsList[playerEntity.weapon - 1].GetComponent<projectileController>().rapid_fire;
            playerEntity.power_control = playerEntity.weaponsList[playerEntity.weapon - 1].GetComponent<projectileController>().power_control;
            playerEntity.inaccuracy = playerEntity.weaponsList[playerEntity.weapon - 1].GetComponent<projectileController>().inaccuracy;
        }


        if (playerEntity.weapon == 0)//bow
        {
            playerEntity.attackVelo = 2f;
            playerEntity.coolDownPeriod = 0.3f;
        }
        if (playerEntity.weapon == -1)//sword
        {
            if (playerEntity.attack == null)
            {
                return;
            }
            playerEntity.attackVelo = playerEntity.attack.GetComponent<projectileController>().attackVelo;
            playerEntity.ManaCost = playerEntity.attack.GetComponent<projectileController>().ManaCost;
            playerEntity.coolDownPeriod = playerEntity.attack.GetComponent<projectileController>().coolDownPeriod;
            playerEntity.rapid_fire = playerEntity.attack.GetComponent<projectileController>().rapid_fire;
            playerEntity.power_control = playerEntity.attack.GetComponent<projectileController>().power_control;
            playerEntity.inaccuracy = playerEntity.attack.GetComponent<projectileController>().inaccuracy;
        }
    }
    public bool IsGrounded()
    {
        Vector2 origin = transform.position;
        origin.y -= 0.5f;
        //Debug.DrawRay(origin, -Vector2.up * 0.05f, Color.red, 0.01f, false);
        return Physics2D.Raycast(origin, -Vector2.up, 0.05f);
    }

    public void TakeDamage(float amount)
    {
        playerEntity.redFlash.SetActive(true);
        playerEntity.spawnedEffect = Instantiate(playerEntity.bloodPuff, transform.position, transform.rotation);
        playerEntity.currentHealth -= amount;
        if (playerEntity.currentHealth <= 0f && !playerEntity.isDead)
        {
            StartCoroutine(DeathDelay(4f));
        }
    }

    private IEnumerator DeathDelay(float interval)
    {
        playerEntity.transition.SetActive(true);
        if (!playerEntity.isDying)
        {
            playerEntity.isDying = true;
            
            playerEntity.BlackBodyParticles.gameObject.SetActive(false);
            playerEntity.skullRB2D.simulated = true;
            playerEntity.handheldWeapon.SetActive(false);
            Instantiate(playerEntity.deathParticles, transform);
        }

        yield return new WaitForSeconds(interval);
        playerEntity.isDead = true;
        yield break;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UpgradeHealth")
        {
            int amount = collision.GetComponent<UpgradeItem>().amount;
            collision.gameObject.SetActive(false);
            Upgrade(0, amount);
        }
        if (collision.tag == "UpgradeMana")
        {
            int amount2 = collision.GetComponent<UpgradeItem>().amount;
            Upgrade(1, amount2);
        }
        if (collision.tag == "UpgradeZoom")
        {
            int amount3 = collision.GetComponent<UpgradeItem>().amount;
            collision.gameObject.SetActive(false);
            Upgrade(2, amount3);
        }
        if (collision.tag == "UpgradeSpeed")
        {
            int amount4 = collision.GetComponent<UpgradeItem>().amount;
            collision.gameObject.SetActive(false);
            Upgrade(3, amount4);
        }
    }

    public void Upgrade(int type, int amount)
    {
        for (int i = amount; i > 0; i--)
        {
            if (type == 0)
            {
                playerEntity.MaxHealth++;
                playerEntity.currentHealth += 1f;
            }
            if (type == 1)
            {
                playerEntity.MaxMana++;
                playerEntity.currentMana++;
            }
            if (type == 2)

            {
                playerEntity.cameraSize += 0.4f;
                playerEntity.Camera.GetComponent<Camera>().orthographicSize = playerEntity.cameraSize;
            }
            if (type == 3)
            {
                playerEntity.speed += 0.5f;
                playerEntity.jumpForce += 0.4f;
            }

            if (playerEntity.health.maxValue < playerEntity.MaxHealth)
            {
                if (playerEntity.MaxHealth < playerEntity.HealthBarScalingLength)
                {
                    playerEntity.healthRect.sizeDelta = new Vector2((float)playerEntity.health.maxValue, playerEntity.healthRect.sizeDelta.y);
                }
                else
                {
                    playerEntity.healthRect.sizeDelta = new Vector2((float)playerEntity.HealthBarScalingLength, playerEntity.healthRect.sizeDelta.y);
                }

                playerEntity.health.maxValue += 1;
            }

            if (playerEntity.mana.maxValue < playerEntity.MaxMana)
            {
                if (playerEntity.MaxMana < playerEntity.HealthBarScalingLength)
                {
                    playerEntity.manaRect.sizeDelta = new Vector2((float)playerEntity.mana.maxValue, playerEntity.manaRect.sizeDelta.y);
                }
                else
                {
                    playerEntity.manaRect.sizeDelta = new Vector2((float)playerEntity.HealthBarScalingLength, playerEntity.manaRect.sizeDelta.y);
                }

                playerEntity.mana.maxValue += 1;
            }
        }
    }
    public void UpdateHealth()
    {
        playerEntity.LivesUI.text = "souls. " + playerEntity.Souls.ToString();

        playerEntity.HealthUIText.text = playerEntity.currentHealth.ToString() + "/" + playerEntity.MaxHealth.ToString();
        //playerEntity.HealthUIText.rectTransform.position = new Vector3(playerEntity.healthRect.sizeDelta.x + 2, playerEntity.HealthUIText.rectTransform.position.y, playerEntity.HealthUIText.rectTransform.position.z);

        playerEntity.ManaUIText.text = playerEntity.currentMana.ToString() + "/" + playerEntity.MaxMana.ToString();
        //playerEntity.ManaUIText.rectTransform.position = new Vector3(playerEntity.manaRect.sizeDelta.x + 2, playerEntity.ManaUIText.rectTransform.position.y, playerEntity.ManaUIText.rectTransform.position.z);

        playerEntity.health.value = Mathf.SmoothStep(playerEntity.health.value, playerEntity.currentHealth, 0.25f);
        playerEntity.mana.value = Mathf.SmoothStep(playerEntity.mana.value, playerEntity.currentMana, 0.25f);
    }

    /*
    public IEnumerator JumpStretch()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.0001f);
            GetComponent<SpriteRenderer>().size = new Vector2(GetComponent<SpriteRenderer>().size.x - 0.02f, GetComponent<SpriteRenderer>().size.y + 0.02f);
        }
        for (int i = 0; i < 10; i++)
        {
            //yield return new WaitForSeconds(0.00001f);
            GetComponent<SpriteRenderer>().size = new Vector2(GetComponent<SpriteRenderer>().size.x + 0.02f, GetComponent<SpriteRenderer>().size.y - 0.02f);
        }
        yield break;
    }
    */

    public void Flinching()
    {
        StartCoroutine(PauseAfterHit());
    }

    private IEnumerator PauseAfterHit()
    {
        playerEntity.Flinch = true;
        yield return new WaitForSeconds(0.2f);
        playerEntity.Flinch = false;
        yield break;
    }

    private float TiltAngle;
    private float TiltTargetAngle;
    private float currVelo;

    public void setTiltTargetAngle(float value)
    {
        TiltTargetAngle = value;
    }
    private void TiltCharacter()
    {
        TiltAngle = playerEntity.BlackBodyParticles.localEulerAngles.z;
        TiltAngle = Mathf.SmoothDampAngle(TiltAngle, TiltTargetAngle, ref currVelo, 0.1f);
        playerEntity.BlackBodyParticles.localEulerAngles = new Vector3(0f, 0f, TiltAngle);
    }
}
