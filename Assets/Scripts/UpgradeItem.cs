using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeItem : MonoBehaviour
{
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnlocksCharging)
            {
                player.GetComponent<PlayerEntity>().Chargeable[UnlockingWeapon - 1] = UnlockingWeapon;
                player.GetComponent<PlayerEntity>().Chargeable[UnlockingWeapon + 7 - 1] = UnlockingWeapon + 7;
            }
            if (UnlockWeapon)
            {
                if (UnlockingWeapon == 1)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponRedUI.SetActive(true);
                }
                else if (UnlockingWeapon == 2)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponOrangeUI.SetActive(true);
                }
                else if (UnlockingWeapon == 3)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponYellowUI.SetActive(true);
                }
                else if (UnlockingWeapon == 4)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponGreenUI.SetActive(true);
                }
                else if (UnlockingWeapon == 5)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponBlueUI.SetActive(true);
                }
                else if (UnlockingWeapon == 6)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponIndigoUI.SetActive(true);
                }
                else if (UnlockingWeapon == 7)
                {
                    Weapons.GetComponent<WeaponUI>().WeaponVioletUI.SetActive(true);
                }

                player.GetComponent<PlayerEntity>().weapon = UnlockingWeapon;
            }
            gameObject.SetActive(false);

            if (gameObject.tag == "UpgradeHealth")
            {
                ShowText("++ " + amount, 2.5f, Color.red);
            }
            if (gameObject.tag == "UpgradeMana")
            {
                ShowText("++ " + amount, 2.5f, Color.cyan);
            }

            if (UnlockedText != null)
            {
                UnlockedText.SetActive(true);
            }
            if (Image != null)
            {
                Image.SetActive(true);
            }
        }
    }

    private void ShowText(string text, float size, Color colour)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        GameObject gameObject2 = Instantiate(TextPopUp, position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = text;
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-30.0f, 30.0f));
        gameObject2.GetComponent<TMPro.TextMeshPro>().color = colour;
    }

    public GameObject TextPopUp;

    public int amount;

    public GameObject UnlockedText;

    public GameObject Image;

    public int UnlockingWeapon;

    public GameObject Weapons;

    public bool UnlocksCharging;

    public bool UnlockWeapon;

    private GameObject player;
}