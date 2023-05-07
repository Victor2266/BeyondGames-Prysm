using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinButton : MonoBehaviour
{
    public float topYpos;
    public float botYpos;
    public LevelManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);

            LeanTween.moveY(gameObject, botYpos, 1f).setEaseInOutExpo().setOnComplete(manager.WinLevel);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveY(gameObject, topYpos, 1f).setEaseInOutExpo();
        }
    }
}
