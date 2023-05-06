using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBloodStainsButton : MonoBehaviour
{
    public BloodManager bloodManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);

            LeanTween.moveY(gameObject, -2.5f, 1f).setEaseInOutExpo().setOnComplete(bloodManager.clearStains);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveY(gameObject, -1.5f, 1f).setEaseInOutExpo();
        }
    }
}

