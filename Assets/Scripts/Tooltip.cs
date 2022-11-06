using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public TextMeshProUGUI statNamesField;
    public TextMeshProUGUI statField;//reach length, damage scale, max damg, swingtime, reset time, special cooldown
    public LayoutElement layoutElement;
    
    private Vector2 position;
    private Vector2 pivot;
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetText(string header, string content, float[] stats, string statNames)
    {
        contentField.text = content;
        headerField.text = header;

        layoutElement.enabled = (headerField.preferredWidth > layoutElement.preferredWidth || contentField.preferredWidth > layoutElement.preferredWidth) ? true : false;


        statField.text = "";
        foreach (float stat in stats)
        {
            statField.text += Math.Round(stat, 2).ToString() + "\n";
        }

        statNamesField.text = statNames;
    }
    public void SetText(string header, string content)
    {
        contentField.text = content;
        headerField.text = header;

        layoutElement.enabled = (headerField.preferredWidth > layoutElement.preferredWidth || contentField.preferredWidth > layoutElement.preferredWidth) ? true : false;


        statField.text = "";
        statNamesField.text = "";
    }

    private void Update()
    {
        position = Input.mousePosition;

        transform.position = position;

        pivot = new Vector2(position.x / Screen.width, position.y / Screen.height);
        rectTransform.pivot = new Vector2(pivot.x, pivot.y);
    }
}
