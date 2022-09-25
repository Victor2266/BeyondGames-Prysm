using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public TextMeshProUGUI statNamesField;
    public TextMeshProUGUI statField;//reach length, damage scale, max damg, swingtime, reset time, special cooldown
    public LayoutElement layoutElement;

    public int characterWrapLimit;


    public void SetText(string header, string content, float[] stats, string statNames)
    {
        contentField.text = content;
        headerField.text = header;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;


        statField.text = "";
        foreach (float stat in stats)
        {
            statField.text += stat.ToString() + "\n";
        }

        statNamesField.text = statNames;
    }
}
