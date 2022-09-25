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
    
    private Vector2 position;

    public void SetText(string header, string content, float[] stats, string statNames)
    {
        contentField.text = content;
        headerField.text = header;

        layoutElement.enabled = (headerField.preferredWidth > layoutElement.preferredWidth || contentField.preferredWidth > layoutElement.preferredWidth) ? true : false;


        statField.text = "";
        foreach (float stat in stats)
        {
            statField.text += stat.ToString() + "\n";
        }

        statNamesField.text = statNames;
    }

    private void Update()
    {
        position = Input.mousePosition;

        transform.position = position;

    }
}
