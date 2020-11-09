using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetButtonDown("Use/Interact") && closed)
        {
            closed = false;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            GameObject gameObject = Instantiate(item, pos, transform.rotation);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 5f);
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

    public GameObject item;

    public bool closed = true;

    public Sprite openedSprite;
}
