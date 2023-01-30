using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTextMessage : MonoBehaviour
{
    public GameObject lastMSG;
    public Transform followThis;
    public float textYOffset;
    public void moveLastMSG()
    {
        if (lastMSG != null)
        {
            if (followThis == null)
                lastMSG.transform.localPosition = new Vector3(lastMSG.transform.localPosition.x, lastMSG.transform.localPosition.y + 0.3f, lastMSG.transform.localPosition.z);
            else
                textYOffset += 0.3f;
        }
        if (lastMSG.GetComponent<InGameTextMessage>().lastMSG != null)
        {
            lastMSG.GetComponent<InGameTextMessage>().moveLastMSG();
        }
    }

    public void Update()
    {
        if (followThis != null)
        {
            transform.position = new Vector3(followThis.transform.localPosition.x, followThis.transform.localPosition.y + textYOffset, transform.position.z);
        }
    }
}
