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

        playerEntity.OrbPosition = playerEntity.Orb.GetComponent<CameraController>();

        SetWeap();
        playerEntity.ManaCost = 0;
        playerEntity.rb2d = GetComponent<Rigidbody2D>();
        playerEntity.anim = GetComponent<Animator>();
        playerEntity.SprtRnderer = GetComponent<SpriteRenderer>();
        
        playerEntity.Camera.GetComponent<Camera>().orthographicSize = playerEntity.cameraSize;

        playerEntity.currentHealth = MySceneManager.StartingHealth;
        playerEntity.currentMana = MySceneManager.StartingMana;

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


        if (playerEntity.weapon == -1)//beginning light weapon
        {
            playerEntity.attackVelo = 2f;
            playerEntity.coolDownPeriod = 0.3f;
        }
    }
    public bool IsGrounded()
    {
        Vector2 origin = transform.position;
        origin.y -= 0.6f;
        return Physics2D.Raycast(origin, -Vector2.up, 0.005f);
    }

    public void TakeDamage(float amount)
    {
        playerEntity.redFlash.SetActive(true);
        playerEntity.spawnedEffect = Instantiate(playerEntity.bloodPuff, transform.position, transform.rotation);
        playerEntity.currentHealth -= amount;
        if (playerEntity.currentHealth <= 0f && !playerEntity.isDead)
        {
            StartCoroutine(DeathDelay(6f));
        }
    }

    private IEnumerator DeathDelay(float interval)
    {
        playerEntity.transition.SetActive(true);
        if (!playerEntity.isDying)
        {
            playerEntity.anim.SetTrigger("Die");
            playerEntity.isDying = true;

            playerEntity.cape.SetActive(false);
        }
        yield return new WaitForSeconds(interval);
        playerEntity.MaxHealth = 100;
        playerEntity.MaxMana = 100;
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
        playerEntity.LivesUI.text = "Lives: " + playerEntity.Lives.ToString();

        playerEntity.HealthUIText.text = playerEntity.currentHealth.ToString() + "/" + playerEntity.MaxHealth.ToString();
        playerEntity.HealthUIText.rectTransform.position = new Vector3(playerEntity.healthRect.sizeDelta.x + 2, playerEntity.HealthUIText.rectTransform.position.y, playerEntity.HealthUIText.rectTransform.position.z);

        playerEntity.ManaUIText.text = playerEntity.currentMana.ToString() + "/" + playerEntity.MaxMana.ToString();
        playerEntity.ManaUIText.rectTransform.position = new Vector3(playerEntity.manaRect.sizeDelta.x + 2, playerEntity.ManaUIText.rectTransform.position.y, playerEntity.ManaUIText.rectTransform.position.z);

        playerEntity.health.value = Mathf.SmoothStep(playerEntity.health.value, playerEntity.currentHealth, 0.25f);
        playerEntity.mana.value = Mathf.SmoothStep(playerEntity.mana.value, playerEntity.currentMana, 0.25f);
    }

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
}
