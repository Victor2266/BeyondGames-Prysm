using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangePromptOnEnable : MonoBehaviour
{
    [SerializeField]
    public string[] Prompts;
    public TextMeshProUGUI textMesh;


    private void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = Prompts[Random.Range(0, Prompts.Length)];
    }
}
