using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetControlSize : MonoBehaviour
{
    public RectTransform rectTrans;
    public float startingSize;
    public float defaultJoyRange = 50;
    public float maxSizeMultiplier = 1;

    public bool changePosY = false;
    public bool changeJoystickRange = false;
    // Start is called before the first frame update
    void Start()
    {
        setSize();
        OptionsMenu.onControlChange += (setSize);
    }
    private void OnDestroy()
    {
        OptionsMenu.onControlChange -= (setSize);
    }

    void setSize()
    {
        rectTrans = GetComponent<RectTransform>();

        float sizePref = startingSize + startingSize * (PlayerPrefs.GetFloat("controlSize", 0) * maxSizeMultiplier);
        rectTrans.sizeDelta = new Vector2(sizePref, sizePref);
        if (changePosY)
        {
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, sizePref);
        }
        
        if (changeJoystickRange)
        {
            GetComponent<UnityEngine.InputSystem.OnScreen.OnScreenStick>().movementRange = defaultJoyRange *(PlayerPrefs.GetFloat("controlSize", 0) + 1);
        }
    }
}
