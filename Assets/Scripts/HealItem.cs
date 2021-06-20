using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int HealAmount;
    public GameObject TextPopUp;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            ShowText("+" + HealAmount, 2, Color.red);
            for (int i = HealAmount; i > 0; i--)
            {
                if (collision.gameObject.GetComponent<PlayerEntity>().currentHealth < (float)collision.gameObject.GetComponent<PlayerEntity>().MaxHealth)
                {
                    collision.gameObject.GetComponent<PlayerEntity>().currentHealth += 1f;
                }
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
}
