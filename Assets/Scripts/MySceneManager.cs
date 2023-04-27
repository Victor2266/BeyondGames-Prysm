using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.InputSystem;
public class MySceneManager : MonoBehaviour
{
    public GameObject transition;
    public PlayerEntity playerEntity = null;
    #region Singleton
    public static MySceneManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    // called first
    void OnEnable()
    {
        transition = GameObject.FindGameObjectWithTag("Transition");
        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DontDestroyOnLoad(this.gameObject);
        transition = GameObject.FindGameObjectWithTag("Transition");
        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
        if(transform != null)
            transition.SetActive(false);
    }

    private void Update()
    {
        if(playerEntity == null)
        {
            return;
        }
        if (playerEntity.isDead) //Player death 
        {
            StartCoroutine(SelectLevelScreen(false));
            playerEntity.isDead = false;
            transition.SetActive(true);
                //SaveSystem.SavePlayerEntity(playerEntity);
        }
    }
    public void PreStartWarning()
    {
        base.StartCoroutine(SlowSelectLevel("NewGameNote"));
    }
    public void StartNewSingleplayerGame()
    {
        //CLEAR EVERYTHING
        ClearData();
       
        base.StartCoroutine(SelectLevelScreen(true));
    }

    public void ContinueSingleplayerGame()
    {
        //LOAD EVERYTHING
        if (LoadData())
        {
            //on successs
            base.StartCoroutine(SelectLevelScreen(false));
        }
        else
        {
            PreStartWarning();
        }
    }

    public IEnumerator SelectLevelScreen(bool needSave)
    {
        transition.SetActive(true);
        Debug.Log("Returning to Level Select");

        yield return new WaitForSeconds(1f);
        if (needSave)
        {
            //SAVE EVERYTHING
            SaveData();

            yield return new WaitForSeconds(3f);
        }
        
        yield return new WaitForSeconds(1f);//temp lowered

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
    public IEnumerator SlowSelectLevel(string levelname)
    {
        transition.SetActive(true);

        yield return new WaitForSeconds(2f);//temp lowered

        //LOAD EVERYTHING
        LoadData();

        yield return new WaitForSeconds(0.2f);//temp lowered

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelname, LoadSceneMode.Single);

        yield break;
    }
    public IEnumerator CompleteLevel(string UnlockNextLevel)
    {
        StartCoroutine(SelectLevelScreen(true));

        yield break;
    }

    public void SaveData()
    {
        SaveSystem.SavePlayerEntity(playerEntity);
        SaveSystem.SaveEquipment();
        SaveSystem.SaveInventory();
        //SAVE LEVEL PROGRESS NOT DONE
    }
    public bool LoadData()
    {
        if (playerEntity == null)
        {
            return true;
        }
        PlayerEntity tempPlayer = SaveSystem.LoadPlayerEntity(playerEntity);
        //SaveSystem.LoadEquipment();
        //Inventory.instance.items = SaveSystem.LoadInventory();
        //LOAD LEVEL PROGRESS [NOT DONE]

        if (tempPlayer != null)
        {
            playerEntity = tempPlayer;
            return true;
        }
        else
        {
            ClearData();
            return false;
        }
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

        //PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("levelReached", 4);
        PlayerPrefs.SetFloat("BGM_Volume", 1f);

        if (Application.platform == RuntimePlatform.Android)
        {
            PlayerPrefs.SetFloat("UISize", 1280 + 636/2f);
            playerEntity.gameObject.GetComponent<PlayerInput>().actions["Jump"].ApplyBindingOverride("<Gamepad>/leftStick/up", path: "<Gamepad>/leftShoulder");
        }

    }
}
