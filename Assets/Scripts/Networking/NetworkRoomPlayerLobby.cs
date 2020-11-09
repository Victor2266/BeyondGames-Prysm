using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;
public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private GameObject[] playerIcons = new GameObject[4];

    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]

    public string DisplayName = "Loading . . .";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    [SerializeField] private GameObject[] SettingOptions = new GameObject[12];
    //Setting Options
    [SyncVar]
    private int StartingHealthValue;
    [SyncVar]
    private int StartingManaValue;
    [SyncVar]
    public bool RegenHealthBool;
    [SyncVar]
    public int RegenHealthValue;
    [SyncVar]
    public bool RegenManabool;
    [SyncVar]
    public int RegenManaValue;
    [SyncVar]
    public bool RegenManaUnlimitedBool;
    [SyncVar]
    public bool LimitedTimeBool;
    [SyncVar]
    public float LimitedTimeValue;
    [SyncVar]
    public bool LimitedStockBool;
    [SyncVar]
    public int LimitedStockValue;
    [SyncVar]
    public int NumberOfWeapons;

    private MyNetworkManagerLobby room;

    private MyNetworkManagerLobby Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton as MyNetworkManagerLobby;
        }
    }
    
    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        //Default Starting Setting Values
        StartingHealthValue = 100;
        StartingManaValue = 100;
        RegenHealthBool = false;
        RegenHealthValue = 1;
        RegenManabool = false;
        RegenManaValue = 1;
        RegenManaUnlimitedBool = false;
        LimitedTimeBool = false;
        LimitedTimeValue = 5;
        LimitedStockBool = false;
        LimitedStockValue = 3;
        NumberOfWeapons = 7;

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        Debug.Log($"added this {this}");
        ClientUpdateGameSettings();
        UpdateDisplay();

        if (!isLeader)
        {
            //Deactive Interacting with the settings for clients
            SettingOptions[0].GetComponent<TMPro.TMP_InputField>().interactable = false;
            SettingOptions[1].GetComponent<TMPro.TMP_InputField>().interactable = false;
            SettingOptions[2].GetComponent<Toggle>().interactable = false;
            SettingOptions[3].GetComponent<TMPro.TMP_InputField>().interactable = false;
            SettingOptions[4].GetComponent<Toggle>().interactable = false;
            SettingOptions[5].GetComponent<TMPro.TMP_InputField>().interactable = false;
            SettingOptions[6].GetComponent<Toggle>().interactable = false;
            SettingOptions[7].GetComponent<Toggle>().interactable = false;
            SettingOptions[8].GetComponent<TMPro.TMP_InputField>().interactable = false;
            SettingOptions[9].GetComponent<Toggle>().interactable = false;
            SettingOptions[10].GetComponent<TMPro.TMP_InputField>().interactable = false;
            SettingOptions[11].GetComponent<TMPro.TMP_Dropdown>().interactable = false;
        }
    }

    public override void OnNetworkDestroy()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach(var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i< playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting . . .";
            playerReadyTexts[i].text = string.Empty;
            playerIcons[i].SetActive(false);
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "Ready" : string.Empty;

            playerIcons[i].SetActive(true);
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader)
        {
            return;
        }

        startGameButton.interactable = readyToStart;
    }

    [Command]

    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]

    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]

    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
        {
            return;
        }

        Room.StartGame();
    }
    public int convertStringToInt(string stringValue, int settingNum)
    {
        if (stringValue.Length >= 6)
        {
            SettingOptions[settingNum].GetComponent<TMPro.TMP_InputField>().text = SettingOptions[settingNum].GetComponent<TMPro.TMP_InputField>().text.Substring(0, 5);
            stringValue = stringValue.Substring(0, 5);
        }

        int result;
        if (int.TryParse(stringValue, out result) == false)
        {
            Debug.Log($"Error Setting {settingNum} had Invalid Character " + connectionToClient + " Value: " + stringValue);
            return 1;

        }
        if (result <= 0)
        {
            Debug.Log($"Error Setting {settingNum} had negative value " + connectionToClient + " Value: " + stringValue);
            return 1;
        }

        return result;
    }

    public void UpdateStartHealth(string stringValue)
    {
        int result = convertStringToInt(stringValue, 0);
        
        StartingHealthValue = result;
        ClientUpdateGameSettings();
    }

    public void UpdateStartMana(string stringValue)
    {
        int result = convertStringToInt(stringValue, 1);


        StartingManaValue = result;
        ClientUpdateGameSettings();
    }

    public void UpdateRegenHealthBool(bool RegenBool)
    {
        RegenHealthBool = RegenBool;
        ClientUpdateGameSettings();
    }

    public void UpdateRegenHealthValue(string stringValue)
    {
        int result = convertStringToInt(stringValue, 3);


        RegenHealthValue = result;
        ClientUpdateGameSettings();
    }

    public void UpdateRegenManaBool(bool RegenBool)
    {
        RegenManabool = RegenBool;
        ClientUpdateGameSettings();
    }

    public void UpdateRegenManaValue(string stringValue)
    {
        int result = convertStringToInt(stringValue, 5);


        RegenManaValue = result;
        ClientUpdateGameSettings();
    }

    public void UpdateRegenManaUnlimitedBool(bool RegenBool)
    {
        RegenManaUnlimitedBool = RegenBool;
        ClientUpdateGameSettings();
    }

    public void UpdateLimitedTimeBool(bool boolVal)
    {
        LimitedTimeBool = boolVal;
        ClientUpdateGameSettings();
    }

    public void UpdateLimitedTimeValue(string stringValue)
    {
        if (stringValue.Length >= 6)
        {
            SettingOptions[8].GetComponent<TMPro.TMP_InputField>().text = SettingOptions[8].GetComponent<TMPro.TMP_InputField>().text.Substring(0, 5);
            stringValue = stringValue.Substring(0, 5);
        }

        float result;
        if (float.TryParse(stringValue, out result) == false)
        {
            Debug.Log("Error Starting Time Value Invalid Character" + connectionToClient);
            result = 5;

        }
        if (result <= 0)
        {
            Debug.Log("Error Starting Time Value Is Negative" + connectionToClient);
            result = 5;
        }


        LimitedTimeValue = result;
        ClientUpdateGameSettings();
    }

    public void UpdateLimitedStockBool(bool boolVal)
    {
        LimitedStockBool = boolVal;
        ClientUpdateGameSettings();
    }

    public void UpdateLimitedStockValue(string stringValue)
    {
        int result = convertStringToInt(stringValue, 10);


        LimitedStockValue = result;
        ClientUpdateGameSettings();
    }

    public void UpdateNumOfWeapons(int result)
    {

        NumberOfWeapons = 7 - result;
        ClientUpdateGameSettings();
    }
    public void ClientUpdateGameSettings()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.CmdUpdateGameSettings();
                    break;
                }
            }

            return;
        }
    }

    [Command]
    public void CmdUpdateGameSettings()//this gets called on the server at the end of the function call editing each of the settings
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.CmdUpdateGameSettings();
                    break;
                }
            }

            return;
        }
        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            Room.RoomPlayers[i].StartingHealthValue = StartingHealthValue;
            Room.RoomPlayers[i].StartingManaValue = StartingManaValue;
            Room.RoomPlayers[i].RegenHealthBool = RegenHealthBool;
            Room.RoomPlayers[i].RegenHealthValue = RegenHealthValue;
            Room.RoomPlayers[i].RegenManabool = RegenManabool;
            Room.RoomPlayers[i].RegenManaValue = RegenManaValue;
            Room.RoomPlayers[i].RegenManaUnlimitedBool = RegenManaUnlimitedBool;
            Room.RoomPlayers[i].LimitedTimeBool = LimitedTimeBool;
            Room.RoomPlayers[i].LimitedTimeValue = LimitedTimeValue;
            Room.RoomPlayers[i].LimitedStockBool = LimitedStockBool;
            Room.RoomPlayers[i].LimitedStockValue = LimitedStockValue;
            Room.RoomPlayers[i].NumberOfWeapons = NumberOfWeapons;
        }
    }

    private void Update()//This makes sure that the setting menu is always synced
    {
        if (SettingOptions[0].GetComponent<TMPro.TMP_InputField>().text != StartingHealthValue.ToString())
        {
            SettingOptions[0].GetComponent<TMPro.TMP_InputField>().text = StartingHealthValue.ToString();
        }
        if (SettingOptions[1].GetComponent<TMPro.TMP_InputField>().text != StartingManaValue.ToString())
        {
            SettingOptions[1].GetComponent<TMPro.TMP_InputField>().text = StartingManaValue.ToString();
        }
        if (SettingOptions[2].GetComponent<Toggle>().isOn != RegenHealthBool)
        {
            SettingOptions[2].GetComponent<Toggle>().isOn = RegenHealthBool;
        }
        if (SettingOptions[3].GetComponent<TMPro.TMP_InputField>().text != RegenHealthValue.ToString())
        {
            SettingOptions[3].GetComponent<TMPro.TMP_InputField>().text = RegenHealthValue.ToString();
        }
        if (SettingOptions[4].GetComponent<Toggle>().isOn != RegenManabool)
        {
            SettingOptions[4].GetComponent<Toggle>().isOn = RegenManabool;
        }
        if (SettingOptions[5].GetComponent<TMPro.TMP_InputField>().text != RegenManaValue.ToString())
        {
            SettingOptions[5].GetComponent<TMPro.TMP_InputField>().text = RegenManaValue.ToString();
        }
        if (SettingOptions[6].GetComponent<Toggle>().isOn != RegenManaUnlimitedBool)
        {
            SettingOptions[6].GetComponent<Toggle>().isOn = RegenManaUnlimitedBool;
        }
        if (SettingOptions[7].GetComponent<Toggle>().isOn != LimitedTimeBool)
        {
            SettingOptions[7].GetComponent<Toggle>().isOn = LimitedTimeBool;
        }
        if (SettingOptions[8].GetComponent<TMPro.TMP_InputField>().text != LimitedTimeValue.ToString())
        {
            SettingOptions[8].GetComponent<TMPro.TMP_InputField>().text = LimitedTimeValue.ToString();
        }
        if (SettingOptions[9].GetComponent<Toggle>().isOn != LimitedStockBool)
        {
            SettingOptions[9].GetComponent<Toggle>().isOn = LimitedStockBool;
        }
        if (SettingOptions[10].GetComponent<TMPro.TMP_InputField>().text != LimitedStockValue.ToString())
        {
            SettingOptions[10].GetComponent<TMPro.TMP_InputField>().text = LimitedStockValue.ToString();
        }
        if (SettingOptions[11].GetComponent<TMPro.TMP_Dropdown>().value != 7 - NumberOfWeapons)
        {
            SettingOptions[11].GetComponent<TMPro.TMP_Dropdown>().value = 7 - NumberOfWeapons;
        }
    }
}
