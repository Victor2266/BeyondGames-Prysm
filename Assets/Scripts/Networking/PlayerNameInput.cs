using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]

    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        DisplayName = defaultName;
    }

    public void SetPlayerName(string name)
    {
        //continueButton.interactable = !string.IsNullOrEmpty(name);
        // I don't use this
    }

    public void SavePlayerName()
    {
        
        DisplayName = nameInputField.text;
        if (DisplayName.Length > 8)
        {
            DisplayName = DisplayName.Substring(0, 8);
        }
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
