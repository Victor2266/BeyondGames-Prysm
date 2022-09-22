using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulData : MonoBehaviour
{
    public int HealAmount;
    public int ManaAmount;

    private GameObject player;
    public GameObject TextPopUp;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        ShowText("++ " + ManaAmount, 2.5f, Color.cyan, 0.15f);
    //        ShowText("++ " + HealAmount, 2.5f, Color.red, 0.40f);

    //        player.GetComponent<PlayerManager>().Upgrade(0, HealAmount);

    //        player.GetComponent<PlayerManager>().Upgrade(1, ManaAmount);
    //        player.GetComponent<PlayerEntity>().Souls++;
    //        Destroy(gameObject);
    //    }
    //}
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ShowText("++ " + ManaAmount, 2.5f, Color.cyan, 0.15f);
            ShowText("++ " + HealAmount, 2.5f, Color.red, 0.40f);

            player.GetComponent<PlayerManager>().Upgrade(0, HealAmount);

            player.GetComponent<PlayerManager>().Upgrade(1, ManaAmount);
            player.GetComponent<PlayerEntity>().Souls++;
            Destroy(gameObject);
        }
    }

    private void ShowText(string text, float size, Color colour, float yOffset)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        GameObject gameObject2 = Instantiate(TextPopUp, position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = text;
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-30.0f, 30.0f));
        gameObject2.GetComponent<TMPro.TextMeshPro>().color = colour;
    }
}
