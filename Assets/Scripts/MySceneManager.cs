using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public PlayerEntity playerEntity;

    public GameObject GameManager;

    public EquipmentManager equipmentManager;

    public PlayerManager playerManager;

    public Transform pos;

    public GameObject player;

    public GameObject transition;

    #region Singleton
    public static MySceneManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            Destroy(this);
            return;
        }
        instance = this;
    }
    #endregion

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        transition = GameObject.FindGameObjectWithTag("Transition");
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        transition.SetActive(false);
        if (player != null)
        {
            pos = player.GetComponent<Transform>();
            playerEntity = player.GetComponent<PlayerEntity>();
            playerManager = player.GetComponent<PlayerManager>();
        }
        if  (GameManager != null)
        {
            equipmentManager = GameManager.GetComponent<EquipmentManager>();
        }

    }

    private void Update()
    {
        if (playerEntity.isDead) //Player death 
        {
            StartCoroutine(SelectLevelScreen());
            playerEntity.isDead = false;
                //SaveSystem.SavePlayerEntity(playerEntity);
        }
    }

    public void StartNewSingleplayerGame()
    {
        //CLEAR EVERYTHING
        ClearData();
       
        base.StartCoroutine(SelectLevelScreen());
    }
    public void ContinueSingleplayerGame()
    {
        //LOAD EVERYTHING
        LoadData();
        
        base.StartCoroutine(SelectLevelScreen());
    }

    private IEnumerator SelectLevelScreen()
    {
        transition.SetActive(true);

        yield return new WaitForSeconds(2f);//temp lowered

        //SAVE EVERYTHING
        SaveData();
        
        yield return new WaitForSeconds(2f);//temp lowered

        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Select", LoadSceneMode.Single);

        playerEntity.isDead = false;
        yield break;
    }
    
    public IEnumerator SelectLevel(string levelname)
    {
        transition.SetActive(true);

        yield return new WaitForSeconds(0.2f);//temp lowered

        //LOAD EVERYTHING
        LoadData();

        yield return new WaitForSeconds(0.2f);//temp lowered

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelname, LoadSceneMode.Single);

        yield break;
    }
    public IEnumerator CompleteLevel(string UnlockNextLevel)
    {
        StartCoroutine(SelectLevelScreen());

        yield break;
    }

    public void SaveData()
    {
        SaveSystem.SavePlayerEntity(playerEntity);
        SaveSystem.SaveEquipment();
        SaveSystem.SaveInventory();
        //SAVE LEVEL PROGRESS NOT DONE
    }
    public void LoadData()
    {
        playerEntity = SaveSystem.LoadPlayerEntity(playerEntity);
        //SaveSystem.LoadEquipment();
        //Inventory.instance.items = SaveSystem.LoadInventory();
        //LOAD LEVEL PROGRESS [NOT DONE]
    }

    public void ClearData()
    {
        playerEntity.MaxHealth = 100;
        playerEntity.MaxMana = 100;
        playerEntity.cameraSize = 3.5f;
        playerEntity.speed = 3f;
        playerEntity.jumpForce = 5.3f;//3.8
        playerEntity.isClimbing = false;
        SaveSystem.SavePlayerEntity(playerEntity);

        //Clears equipments and inventory below
        SaveSystem.deleteInventoryAndEquipment();

        //clear level progress [NOT DONE]
    }
}
