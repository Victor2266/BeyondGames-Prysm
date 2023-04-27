using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDummyButton : MonoBehaviour
{
    public Dummy dummy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            LeanTween.cancel(gameObject);

            LeanTween.moveY(gameObject, -2.5f, 1f).setEaseInOutExpo().setOnComplete(dummy.resetTotalDMG);

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
