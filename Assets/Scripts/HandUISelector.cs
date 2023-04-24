using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUISelector : MonoBehaviour
{
    private newPointerScript pointerScript;

    private OscillateUpDown oscilator;

    public float Yoffset;

    // Start is called before the first frame update
    void Start()
    {
        pointerScript = GetComponent<newPointerScript>();
        oscilator = GetComponent<OscillateUpDown>();
    }

    public void setHighlighted(GameObject obj)
    {
        pointerScript.lookAtThis = obj.transform;
        LeanTween.value(gameObject, oscilator.startPos.y, obj.transform.position.y + Yoffset, 0.1f).setOnUpdate((float val) => { oscilator.startPos.y = val; }).setEaseInOutSine();

    }
}
