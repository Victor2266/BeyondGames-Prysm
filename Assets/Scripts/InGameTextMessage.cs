using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTextMessage : MonoBehaviour
{
    public GameObject lastMSG;

    public void moveLastMSG()
    {
        if (lastMSG != null)
        {
            lastMSG.transform.localPosition = new Vector3(lastMSG.transform.localPosition.x, lastMSG.transform.localPosition.y + 0.3f, lastMSG.transform.localPosition.z);
        }
        if (lastMSG.GetComponent<InGameTextMessage>().lastMSG != null)
        {
            lastMSG.GetComponent<InGameTextMessage>().moveLastMSG();
        }
    }
}
