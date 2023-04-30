using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinButton : MonoBehaviour
{
    public TrainingRoomManager manager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);

            LeanTween.moveY(gameObject, -2.25f, 1f).setEaseInOutExpo().setOnComplete(manager.WinLevel);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveY(gameObject, -1.75f, 1f).setEaseInOutExpo();
        }
    }
}
