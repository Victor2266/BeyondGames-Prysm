using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWeapon : MonoBehaviour
{
    public ImperialKnightLongSword IKLS;
    public float knockbackX;
    public float knockbackY;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IKLS.LookingLeft)
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-knockbackX, knockbackY);
            else
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(knockbackX, knockbackY);

            IKLS.player.SendMessage("TakeDamage", 1);

            return;
        }
    }
}
