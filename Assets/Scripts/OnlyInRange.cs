using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyInRange : MonoBehaviour
{
    public GameObject RangeConstrained;
    public bool PersistWhenExitRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RangeConstrained.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PersistWhenExitRange)
        {
            if (collision.tag == "Player")
            {
                RangeConstrained.SetActive(false);
            }
        }
        
    }
}
