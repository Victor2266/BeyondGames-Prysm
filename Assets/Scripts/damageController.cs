using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageController : MonoBehaviour
{
    public GameObject DMGText;
    public void ShowDMGText(int damage, float size)
    {
        GameObject gameObject2 = Instantiate(DMGText, transform.position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = damage.ToString();
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-50.0f, 50.0f));

        //NetworkServer.Spawn(gameObject2);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
        }
    }
}
