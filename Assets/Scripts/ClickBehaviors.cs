using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WeaponController
{

    private void defaultLeftClicking()//sword strategy/sling weapon/heavysword
    {
        if (PlayerController.startLeft && timeStamp <= Time.time && !WeaponEnabled)//left click that enables weapon
        {
            lastPosition = transform.position;
            lastZAngle = zAngle;
            weaponUI.flashWhite();
            enableWeapon();
            audioSource.Play();
            totalDistance = MinDamage / DMG_Scaling;
            lastHit = null;

            arrowColor.color = new Color(0f, 0f, 0f, 0f);
        }
        if (PlayerController.holdingLeft && timeStamp <= Time.time && WeaponEnabled)//while holding left click
        {
            currPos = transform.position;
            distance = Vector3.Distance(lastPosition, currPos);//USED IN DAMAGE CALC

            if (distance > ReachLength * 1.3f && DMG > MaxDamage * 0.1)
            {
                audioSource.Play();
            }

            totalDistance += distance;
            DMG = (int)(totalDistance * DMG_Scaling);
            if (DMG > MaxDamage)
            {
                DMG = MaxDamage;
            }
            DamageCounter.text = DMG.ToString();

            if (MaxDamage > 0)
                DamageCounter.color = Color.Lerp(Color.yellow, Color.red, (float)DMG / MaxDamage);
            else
                DamageCounter.color = Color.Lerp(Color.yellow, Color.red, (float)DMG / 100f);

            StaminaBar.value = StaminaBar.maxValue - ((Time.time - startTime) / activeTimeLimit) * StaminaBar.maxValue;

            zAngle = transform.localEulerAngles.z;
            if (distance > capsuleColider.size.x * 2f)//capsule checking
            {
                worldSpaceOffset = new Vector2(-Mathf.Sin(zAngle * Mathf.Deg2Rad) * capsuleColider.offset.y, Mathf.Cos(zAngle * Mathf.Deg2Rad) * capsuleColider.offset.y);//this is the y offset, no xoffset yet
                hits = Physics2D.CapsuleCastAll((Vector2)currPos + worldSpaceOffset, capsuleColider.size, CapsuleDirection2D.Vertical, transform.localEulerAngles.z, lastPosition - currPos, distance);//used to check if passing through hitboxes
                //Debug.DrawRay((Vector2)currPos + worldSpaceOffset, lastPosition - currPos, Color.red, 10.0f);

                foreach (RaycastHit2D hit in hits)
                {

                    if (capsuleColider.IsTouching(hit.collider))//if weapon is in colider, no need for casting
                    {
                        lastHit = hit.collider.gameObject;
                    }
                    if (lastHit == null)
                    {
                        DoubleCheckingCollision(hit);
                    }
                    else if (lastHit != (hit.collider.gameObject))
                    {
                        //Debug.Log(lastHit.name);
                        DoubleCheckingCollision(hit);
                    }

                }
            }

            if (oneSided)
            {
                if (zAngle - lastZAngle > 0)
                {
                    if (!flipSide)
                        transform.localScale = new Vector3(-xySize, xySize, 1f);
                    else
                        transform.localScale = new Vector3(xySize, xySize, 1f);
                }
                else
                {
                    if (!flipSide)
                        transform.localScale = new Vector3(xySize, xySize, 1f);
                    else
                        transform.localScale = new Vector3(-xySize, xySize, 1f);
                }
            }


            lastPosition = currPos;
            lastZAngle = zAngle;
        }
        if ((PlayerController.liftLeft && WeaponEnabled) || (Time.time > startTime + activeTimeLimit && WeaponEnabled))//released left click
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);

            totalDistance = 0f;

            remainingCooldown = cooldownTime - (1f - (Time.time - startTime) / activeTimeLimit) * cooldownTime;
            timeStamp = Time.time + remainingCooldown;
            weaponUI.ScaleDown(remainingCooldown);
            StaminaBar.value = 0f;
            WeaponEnabled = false;
            Trail.SetActive(false);
            DamageCounter.text = "";

            arrowColor.color = new Color(0f, 0f, 0f, 0f);
            whiteArrow.SetActive(true);
        }
    }


    private void defaultRightClicking()
    {
        if (timeStamp <= Time.time)
        {
            if ((PlayerController.startRight || (playerEntity.rapid_fire && PlayerController.holdingRight)) && playerEntity.charges == 1 && playerEntity.currentMana - playerEntity.ManaCost >= 0)
            {
                playerEntity.WeaponUI.ScaleDown(playerEntity.coolDownPeriod);
                weaponUI.flashWhite();
                whiteArrow.SetActive(false);
                sprtrend.color = new Vector4(1f, 1f, 1f, 1f);
                StaminaBar.value = 0f;
                if (projAsChild == true)
                {
                    playerEntity.bullet = Instantiate(playerEntity.attack, transform.position, transform.rotation, transform);
                }
                else
                {
                    playerEntity.bullet = Instantiate(playerEntity.attack, transform.position, transform.rotation);
                }
                playerEntity.setMana(playerEntity.currentMana - playerEntity.ManaCost);
                playerEntity.charges = 0;
                ReachLength = thrustShortReach;
                Dash();
                if (playerEntity.weapon < 8)
                {
                    playerEntity.bullet.GetComponent<projectileController>().Primed = false;
                }
            }
            if (playerEntity.charges == 0)
            {
                if (PlayerController.holdingLeft || PlayerController.holdingRight)
                {
                    if (playerEntity.bullet != null)
                    {
                        Vector3 TransNorm = transform.localPosition.normalized;
                        playerEntity.bullet.transform.position = transform.position - TransNorm * projectileOffset;
                        playerEntity.bullet.transform.eulerAngles = transform.localEulerAngles + spriteTransform.localEulerAngles;

                        if (playerEntity.power_control == false && playerEntity.inaccuracy == 0)
                        {
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerEntity.attackVelo * TransNorm.x, playerEntity.attackVelo * TransNorm.y);
                        }
                        else if (playerEntity.power_control && playerEntity.inaccuracy == 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, playerEntity.CameraOrbObj.transform.position);
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerEntity.attackVelo * TransNorm.x * scaled_pow, playerEntity.attackVelo * TransNorm.y * scaled_pow);
                        }
                        else if (playerEntity.power_control == false && playerEntity.inaccuracy > 0)
                        {
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(playerEntity.attackVelo * TransNorm.x - playerEntity.inaccuracy, playerEntity.attackVelo * TransNorm.x + playerEntity.inaccuracy),
                                Random.Range(playerEntity.attackVelo * TransNorm.y - playerEntity.inaccuracy, playerEntity.attackVelo * TransNorm.y + playerEntity.inaccuracy));
                        }
                        if (playerEntity.power_control && playerEntity.inaccuracy > 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, playerEntity.CameraOrbObj.transform.position);
                            float velo = playerEntity.attackVelo * 2 * scaled_pow;
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(velo * TransNorm.x - playerEntity.inaccuracy, velo * TransNorm.x + playerEntity.inaccuracy),
                                Random.Range(velo * TransNorm.y - playerEntity.inaccuracy, velo * TransNorm.y + playerEntity.inaccuracy));
                        }
                    }

                }
                if (playerEntity.rapid_fire == true)
                {
                    timeStamp = Time.time + playerEntity.coolDownPeriod;
                    playerEntity.charges = 1;
                    if (playerEntity.bullet != null)
                    {
                        playerEntity.bullet.GetComponent<projectileController>().Primed = true;
                    }

                }
                else if (PlayerController.liftLeft || PlayerController.liftRight)
                {
                    timeStamp = Time.time + playerEntity.coolDownPeriod;
                    playerEntity.charges = 1;
                    if (playerEntity.bullet != null)
                    {
                        playerEntity.bullet.GetComponent<projectileController>().Primed = true;
                    }


                }
                sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
            }
        }
    }
}
