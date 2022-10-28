using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerEntity playerEntity;
    public PlayerManager playerManager;


    private void Update()
    {
        if (!playerEntity.customLocalPlayerCheck)
            return;
        if (!playerEntity.isDying)
        {
            MoveUpdate();
            Shooting();
        }
        if (Input.GetButtonDown("Reset"))
        {
            playerManager.TakeDamage(playerEntity.currentHealth);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            playerManager.Upgrade(0, 900);
            playerManager.Upgrade(1, 900);
            //Upgrade(3, 1);
            Debug.Log(playerEntity.health.maxValue);
        }
    }

    public void MoveUpdate()
    {

        if (Input.GetAxisRaw("Horizontal") > 0f && Mathf.Abs(playerEntity.rb2d.velocity.x) < playerEntity.speed && !playerEntity.Flinch)
        {
            //playerEntity.SprtRnderer.flipX = false;
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            playerEntity.lookingLeft = false;
            playerEntity.rb2d.velocity = new Vector2(playerEntity.speed, playerEntity.rb2d.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Mathf.Abs(playerEntity.rb2d.velocity.x) < playerEntity.speed && !playerEntity.Flinch)
        {
            //playerEntity.SprtRnderer.flipX = true;
            //transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            playerEntity.lookingLeft = true;
            playerEntity.rb2d.velocity = new Vector2(-playerEntity.speed, playerEntity.rb2d.velocity.y);
        }
        else if (Mathf.Abs(playerEntity.rb2d.velocity.x) < 0.05f)
        {
            //playerEntity.anim.SetBool("Running", false);

            playerManager.setTiltTargetAngle(0f);
        }/*
        else if (playerEntity.rb2d.velocity.x < -0.5f)
        {
            playerEntity.rb2d.velocity = new Vector2(playerEntity.rb2d.velocity.x + 0.3f, playerEntity.rb2d.velocity.y);
            Debug.Log("decrease LEFT");

            Debug.Log(playerEntity.rb2d.velocity.x);
        }
        else if (playerEntity.rb2d.velocity.x > 0.5f)
        {
            playerEntity.rb2d.velocity = new Vector2(playerEntity.rb2d.velocity.x - 0.3f, playerEntity.rb2d.velocity.y);
            Debug.Log("decrease RIGHT");//this gets activated more

            Debug.Log(playerEntity.rb2d.velocity.x);
        }*/

        if (playerEntity.rb2d.velocity.y < 0f && !playerEntity.isClimbing)
        {
            playerEntity.rb2d.velocity += Vector2.up * Physics2D.gravity.y * (playerEntity.fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (playerEntity.rb2d.velocity.y > 0f && !Input.GetButton("Jump") && !playerEntity.isClimbing)
        {
            playerEntity.rb2d.velocity += Vector2.up * Physics2D.gravity.y * (playerEntity.lowJumpMultiplier - 1f) * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && playerManager.grounded)
        {
            playerEntity.rb2d.velocity = new Vector2(playerEntity.rb2d.velocity.x, playerEntity.jumpForce);
            //StartCoroutine(playerManager.JumpStretch());
            Vector3 GasPos = new Vector3(transform.position.x, transform.position.y - 0.4f, 0);
            playerEntity.spawnedEffect = Instantiate(playerEntity.gasPuff, GasPos, transform.rotation);

            playerEntity.leftBosoter.startLifetime = 0.5f;
            playerEntity.rightBooster.startLifetime = 0.5f;
        }
        if (Input.GetButtonUp("Jump"))
        {
            playerEntity.leftBosoter.startLifetime = 0f;
            playerEntity.rightBooster.startLifetime = 0f;
        }

        if (Input.GetButton("RegenMana") && playerEntity.currentMana < (float)playerEntity.MaxMana && playerEntity.currentHealth - 0.5f > 0)
        {
            playerEntity.setHealthAndMana(playerEntity.currentHealth - 0.5f, playerEntity.currentMana + 2);

        }
        if (Input.GetButtonDown("Slide") && Input.GetAxisRaw("Horizontal") != 0)//testing without ground check  playerManager.IsGrounded() 
        {
            if (playerEntity.SlideCooldown <= Time.time)
            {
                playerEntity.SlideCooldown = Time.time + 0.5f;

                GameObject dash = Instantiate(playerEntity.speedTrail, transform);
                dash.transform.position = transform.position;
                //playerEntity.speedTrail.SetActive(true);

                //playerEntity.Flinch = true;
                //playerEntity.rb2d.velocity = new Vector2(playerEntity.rb2d.velocity.x * 4f, playerEntity.rb2d.velocity.y);



                if (Input.GetAxisRaw("Horizontal") > 0f)
                {
                    //playerEntity.rb2d.velocity = new Vector2(playerEntity.rb2d.velocity.x + 8f, playerEntity.rb2d.velocity.y);
                    playerEntity.rb2d.AddForce(new Vector2(6f, 0f), ForceMode2D.Impulse);
                    dash.transform.eulerAngles = new Vector3(0f, 0f, 90f);

                    playerEntity.leftBosoter.startLifetime = 0.7f;
                    playerEntity.rightBooster.startLifetime = 0.6f;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0f)
                {
                    //playerEntity.rb2d.velocity = new Vector2(playerEntity.rb2d.velocity.x - 8f, playerEntity.rb2d.velocity.y);

                    playerEntity.rb2d.AddForce(new Vector2(-6f, 0f), ForceMode2D.Impulse);
                    dash.transform.eulerAngles = new Vector3(0f, 0f, -90f);

                    playerEntity.leftBosoter.startLifetime = 0.6f;
                    playerEntity.rightBooster.startLifetime = 0.7f;
                }
                //GetComponent<CapsuleCollider2D>().size = new Vector2(0.12f, 0.3f);
                //GetComponent<CapsuleCollider2D>().offset = new Vector2(0.01f, -0.36f);
            }
        }
        if (Input.GetButtonUp("Slide"))
        {
            playerEntity.leftBosoter.startLifetime = 0f;
            playerEntity.rightBooster.startLifetime = 0f;
        }
    }

    public void Shooting()
    {
        if (playerEntity.weapon < 1)
        {
            return;
        }
        if ((Input.GetButtonDown("Charge") || Input.GetMouseButtonDown(1)))
        {
            if (playerEntity.weapon <= 7 && playerEntity.weapon > 0)
            {
                playerEntity.ChargeIndicator.SetActive(true);
                playerEntity.weapon += 7;
                Debug.Log("chrge up");
            }
            else if (playerEntity.weapon >= 8)
            {
                playerEntity.ChargeIndicator.SetActive(false);
                playerEntity.weapon -= 7;
                Debug.Log("chrge down");
            }
            playerManager.SetWeap();
        }

        if (playerEntity.timeStamp <= Time.time)
        {

            if ((Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(1)) ||
                (playerEntity.rapid_fire && (Input.GetMouseButton(0))))
                && playerEntity.charges == 1 && playerEntity.currentMana - playerEntity.ManaCost >= 0)
            {
                playerEntity.WeaponUI.ScaleDown(playerEntity.coolDownPeriod);

                playerEntity.bullet = Instantiate(playerEntity.attack, transform.position, transform.rotation);
                playerEntity.setMana(playerEntity.currentMana - playerEntity.ManaCost);

                playerEntity.charges = 0;
                if (playerEntity.weapon < 8)
                {
                    playerEntity.bullet.GetComponent<projectileController>().Primed = false;
                }
            }
            if (playerEntity.charges == 0)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    if (playerEntity.bullet != null)
                    {
                        Vector2 v = new Vector2(0.5f * Mathf.Cos(Mathf.Deg2Rad * (playerEntity.mousePointer.transform.eulerAngles.z - 90)), 0.5f * Mathf.Sin(Mathf.Deg2Rad * (playerEntity.mousePointer.transform.eulerAngles.z - 90)));
                        playerEntity.bullet.transform.position = new Vector3(v.x + transform.position.x, v.y + transform.position.y, playerEntity.bullet.transform.position.z);
                        playerEntity.bullet.transform.eulerAngles = playerEntity.mousePointer.transform.eulerAngles;

                        if (playerEntity.power_control == false && playerEntity.inaccuracy == 0)
                        {
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerEntity.attackVelo * 2 * v.x, playerEntity.attackVelo * 2 * v.y);
                        }
                        else if (playerEntity.power_control && playerEntity.inaccuracy == 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, playerEntity.CameraOrbObj.transform.position);
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerEntity.attackVelo * 2 * v.x * scaled_pow, playerEntity.attackVelo * 2 * v.y * scaled_pow);
                        }
                        else if (playerEntity.power_control == false && playerEntity.inaccuracy > 0)
                        {
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(playerEntity.attackVelo * 2 * v.x - playerEntity.inaccuracy, playerEntity.attackVelo * 2 * v.x + playerEntity.inaccuracy),
                                Random.Range(playerEntity.attackVelo * 2 * v.y - playerEntity.inaccuracy, playerEntity.attackVelo * 2 * v.y + playerEntity.inaccuracy));
                        }
                        if (playerEntity.power_control && playerEntity.inaccuracy > 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, playerEntity.CameraOrbObj.transform.position);
                            float velo = playerEntity.attackVelo * 2 * scaled_pow;
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(velo * v.x - playerEntity.inaccuracy, velo * v.x + playerEntity.inaccuracy),
                                Random.Range(velo * v.y - playerEntity.inaccuracy, velo * v.y + playerEntity.inaccuracy));
                        }
                        playerEntity.OrbPosition.offsetX = v.x * 0.65f;
                        playerEntity.OrbPosition.offsetY = v.y * 0.65f;
                    }

                }
                if (playerEntity.rapid_fire == true)
                {
                    playerEntity.timeStamp = Time.time + playerEntity.coolDownPeriod;
                    playerEntity.charges = 1;
                    if (playerEntity.bullet != null)
                    {
                        playerEntity.bullet.GetComponent<projectileController>().Primed = true;
                    }

                }
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    playerEntity.timeStamp = Time.time + playerEntity.coolDownPeriod;
                    playerEntity.charges = 1;
                    if (playerEntity.bullet != null)
                    {
                        playerEntity.bullet.GetComponent<projectileController>().Primed = true;
                    }


                }
            }
            //This is the old keyboard aiming
            /*
            if (((Input.GetButtonDown("Horizontal Shooting") || Input.GetButtonDown("Vertical Shooting")) || 
                (rapid_fire && (Input.GetButton("Horizontal Shooting") || Input.GetButton("Vertical Shooting")))) 
                && charges == 1 && currentMana - ManaCost >= 0)
            {
                clone = Instantiate(attack, transform.position, transform.rotation);
                currentMana -= ManaCost;
                charges = 0;
                if (weapon < 8)
                {
                    clone.GetComponent<projectileController>().Primed = false;
                }
            }
            if (charges == 0)
            {
                if (Input.GetButton("Horizontal Shooting") || Input.GetButton("Vertical Shooting"))
                {
                    if (Input.GetAxisRaw("Horizontal Shooting") > 0f)
                    {
                        Vector2 v = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        clone.transform.position = new Vector3 (v.x,v.y, clone.transform.position.z);
                        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(attackVelo, clone.GetComponent<Rigidbody2D>().velocity.y);

                        OrbPosition.offsetX = 0.5f;
                        OrbPosition.offsetY = -1.5f;
                    }
                    if (Input.GetAxisRaw("Horizontal Shooting") < 0f)
                    {
                        Vector2 v2 = new Vector2(transform.position.x - 0.5f, transform.position.y);
                        clone.transform.position = new Vector3(v2.x, v2.y, clone.transform.position.z);
                        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(-attackVelo, clone.GetComponent<Rigidbody2D>().velocity.y);

                        OrbPosition.offsetX = -0.5f;
                        OrbPosition.offsetY = -1.5f;
                    }
                    if (Input.GetAxisRaw("Vertical Shooting") > 0f)
                    {
                        Vector2 v3 = new Vector2(transform.position.x, transform.position.y + 0.5f);
                        clone.transform.position = new Vector3(v3.x, v3.y, clone.transform.position.z);
                        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(clone.GetComponent<Rigidbody2D>().velocity.x, attackVelo);

                        OrbPosition.offsetX = 0;
                        OrbPosition.offsetY = -1f;
                    }
                    if (Input.GetAxisRaw("Vertical Shooting") < 0f)
                    {
                        Vector2 v4 = new Vector2(transform.position.x, transform.position.y - 0.5f);
                        clone.transform.position = new Vector3(v4.x, v4.y, clone.transform.position.z);
                        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(clone.GetComponent<Rigidbody2D>().velocity.x, -attackVelo);

                        OrbPosition.offsetX = 0;
                        OrbPosition.offsetY = -2f;
                    }
                    if (Mathf.Abs(clone.GetComponent<Rigidbody2D>().velocity.x) == Mathf.Abs(clone.GetComponent<Rigidbody2D>().velocity.y))
                    {
                        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(clone.GetComponent<Rigidbody2D>().velocity.x * 0.7071f, clone.GetComponent<Rigidbody2D>().velocity.y * 0.7071f);
                    }
                }
                if (rapid_fire == true)
                {
                    timeStamp = Time.time + coolDownPeriod;
                    charges = 1;
                    clone.GetComponent<projectileController>().Primed = true;
                }
                else if ((Input.GetButtonUp("Horizontal Shooting") || Input.GetButtonUp("Vertical Shooting")))
                {
                    timeStamp = Time.time + coolDownPeriod;
                    charges = 1;
                    clone.GetComponent<projectileController>().Primed = true;
                }
            }*/


            else if (playerEntity.timeStamp + 2f <= Time.time)//RETURNS HAND ORB TO MIDDLE POSITION
            {
                playerEntity.OrbPosition.offsetX = 0f;
                playerEntity.OrbPosition.offsetY = 0.05f;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (playerEntity.weapon >= 8)
            {
                playerEntity.ChargeIndicator.SetActive(false);
                playerEntity.weapon -= 7;
                playerManager.SetWeap();
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "magicItem" && playerEntity.currentMana < playerEntity.MaxMana)
        {
            playerEntity.currentMana += 2;
            if (playerEntity.currentMana % 5 == 0 && playerEntity.currentMana != playerEntity.MaxMana)
            {
                ShowText("+", 2, Color.cyan);
            }

            playerEntity.UpdateMana();
        }
        if (collision.tag == "healthItem" && playerEntity.currentHealth < (float)playerEntity.MaxHealth)
        {
            playerEntity.currentHealth += 2f;
            if ((int)playerEntity.currentHealth % 5 == 0 && playerEntity.currentHealth != playerEntity.MaxHealth)
            {
                ShowText("+", 2, Color.red);
            }

            playerEntity.UpdateHealth();
        }
    }

    private void ShowText(string text, float size, Color colour)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        GameObject gameObject2 = Instantiate(playerEntity.TextPopUp, position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = text;
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-30.0f, 30.0f));
        gameObject2.GetComponent<TMPro.TextMeshPro>().color = colour;
    }
}