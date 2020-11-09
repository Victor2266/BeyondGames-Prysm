using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;
public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private GameObject[] playerNameIconHUDs = new GameObject[4];
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private GameObject[] playerIcons = new GameObject[4];
    [SerializeField] private GameObject[] playerHealthSliders = new GameObject[4];
    [SerializeField] private GameObject[] playerManaSliders = new GameObject[4];

    [SyncVar]
    public string DisplayName = "Loading . . .";
    
    [Header("Settings")]
    public int maxHealth = 100;
    public int maxMana = 100;
 
    [SerializeField]private float healthRatio = 1f;
    [SerializeField]private float manaRatio = 1f;

    #region HealthEvent
    [SyncVar]
    public float currentHealth;

    public delegate void HealthChangedDelegate(float currentHealth, int maxHealth);
    [SyncEvent]
    public event HealthChangedDelegate EventHealthChanged;

    [Server]
    private void SetHealth(float value)
    {
        currentHealth = value;
        EventHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    [Command]
    public void CmdSetHealth(float value)
    {
        SetHealth(value);
    }

    private void HandleHealthChanged(float currentHealth, int maxHealth)
    {
        healthRatio = currentHealth / maxHealth;
        UpdateDisplay();
    }
    #endregion

    #region ManaEvent
    [SyncVar(hook = "OnManaChanged")]
    public int currentMana;

    [Server]
    private void SetMana(int value)
    {
        currentMana = value;
    }

    [Command]
    public void CmdSetMana(int value)
    {
        SetMana(value);
    }

    private void OnManaChanged(int old_currentMana, int new_currentMana)
    {
        currentMana = new_currentMana;
        manaRatio = (float)new_currentMana / maxMana;
        UpdateDisplay();
    }
    #endregion

    #region Sprite_Orientation

    [SyncVar(hook = "OnOrientationChanged")]
    public bool lookingLeft;

    private SpriteRenderer SpRend;

    [Server]
    private void SetOrientation(bool value)
    {
        lookingLeft = value;
    }
    [Command]
    private void CmdCngOrientation(bool val)
    {
        SetOrientation(val);
    }
    private void OnOrientationChanged(bool old_value, bool new_value)
    {
        lookingLeft = new_value;
        SpRend.flipX = lookingLeft;
    }
    #endregion

    private void OnEnable()
    {
        EventHealthChanged += HandleHealthChanged;
    }
    private void OnDisable()
    {
        EventHealthChanged -= HandleHealthChanged;
    }

    public override void OnStartServer()
    {
        SetHealth(maxHealth);
        SetMana(maxMana);
        SetOrientation(false);
    }
    
    private void Start()
    {
        SpRend = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!hasAuthority) { return; }
        
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            CmdCngOrientation(false);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            CmdCngOrientation(true);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            CmdUpdateDisplayInitial();
        }
    }
    public GameObject PlayerNametag;

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
        lobbyUI.SetActive(true);
    }

    public override void OnStartLocalPlayer()
    {
        CmdUpdateDisplayInitial(); //call the cnd yodate disokay initlal when the round actually starts because each player initallizes before registering other players
    }
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
    }
    
    public override void OnNetworkDestroy()
    {
        Room.GamePlayers.Remove(this);

        if (!hasAuthority)
        {
            foreach (var player in Room.GamePlayers)
            {
                if (player.hasAuthority)
                {
                    player.CmdUpdateDisplayInitial();
                    break;
                }
            }

            return;
        }
        CmdUpdateDisplayInitial();
    }

    #region UpdateDisplay Initial
    [Command]
    public void CmdUpdateDisplayInitial()
    {
        RpcUpdateDisplayInitial();
    }
    [ClientRpc]
    private void RpcUpdateDisplayInitial()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.GamePlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplayInitial();
                    break;
                }
            }

            return;
        }
        UpdateDisplayInitial();
    }
    private void UpdateDisplayInitial()
    {
   
        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameIconHUDs[i].SetActive(true);

            playerNameTexts[i].text = "Waiting . . .";
            playerIcons[i].SetActive(false);
        }

        int j = 1;

        for (int i = 0; i < Room.GamePlayers.Count; i++)
        {
            if (this == Room.GamePlayers[i])
            {
                playerNameTexts[0].text = Room.GamePlayers[i].DisplayName;
                //playerHealthSliders[0].GetComponent<Slider>().value = Room.GamePlayers[i].healthRatio;

                PlayerNametag.SetActive(false);
                playerIcons[0].SetActive(true);
            }
            else
            {
                playerNameTexts[j].text = Room.GamePlayers[i].DisplayName;
                playerHealthSliders[j].GetComponent<Slider>().value = Room.GamePlayers[i].healthRatio;
                playerManaSliders[j].GetComponent<Slider>().value = Room.GamePlayers[i].manaRatio;
                playerIcons[j].SetActive(true);

                Room.GamePlayers[i].SpRend.flipX = Room.GamePlayers[i].lookingLeft;
                Room.GamePlayers[i].PlayerNametag.GetComponent<TMPro.TextMeshPro>().text = Room.GamePlayers[i].DisplayName;
                j++;
            }
            /*playerNameTexts[i].text = Room.GamePlayers[i].DisplayName;
            Room.GamePlayers[i].PlayerNametag.GetComponent<TMPro.TextMeshPro>().text = Room.GamePlayers[i].DisplayName;
            playerIcons[i].SetActive(true);*/
        }
        for (int i = 3; i >= Room.GamePlayers.Count; i--)
        {
            playerNameIconHUDs[i].SetActive(false);
        }
    }

    #endregion
    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach(var player in Room.GamePlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }
        
        int j = 1;

        for (int i = 0; i < Room.GamePlayers.Count; i++)
        {
            if(this == Room.GamePlayers[i])
            {
                
            }
            else
            {
                playerHealthSliders[j].GetComponent<Slider>().value = Room.GamePlayers[i].healthRatio;
                playerManaSliders[j].GetComponent<Slider>().value = Room.GamePlayers[i].manaRatio;
                
                j++;
            }
            /*playerNameTexts[i].text = Room.GamePlayers[i].DisplayName;
            Room.GamePlayers[i].PlayerNametag.GetComponent<TMPro.TextMeshPro>().text = Room.GamePlayers[i].DisplayName;
            playerIcons[i].SetActive(true);*/
        }
    }
    
    [Server]
    public void SetDisplayName(string displayName)
    {
        this.DisplayName = displayName;
    }
}
