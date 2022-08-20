using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleButton : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (this.GoingUp)
            {
                this.Elevator.SendMessage("GoTop");
            }
            if (!this.GoingUp)
            {
                this.Elevator.SendMessage("GoBot");
            }
        }
    }

    public GameObject Elevator;

    public bool GoingUp;
}
