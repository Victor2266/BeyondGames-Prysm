using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
    public Tooltip toolTip;

    public void Awake()
    {
        instance = this;
    }

    public static void Show(string header, string content, float[] stats, string StatNames)
    {
        instance.toolTip.SetText(header, content, stats, StatNames);
        instance.toolTip.gameObject.SetActive(true);
    }
    public static void Show(string header, string content)
    {
        instance.toolTip.SetText(header, content);
        instance.toolTip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        instance.toolTip.gameObject.SetActive(false);
    }
}
