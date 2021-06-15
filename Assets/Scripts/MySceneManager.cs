using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private static PlayerEntity playerEntity;

    public GameObject player;

    private Transform pos;

    public GameObject transition;

    public static int StartingHealth;

    public static int StartingMana;

    private static float StartingSpeed;

    private static float StartingSize;

    public static float StartingJump;

    private static int[] StartingChargeable = new int[14];

    public static int CheckpointHealth;

    public static int CheckpointMana;

    private static float CheckpointSpeed;

    private static float CheckpointSize;

    public static float CheckpointJump;

    private static int[] CheckpointChargeable = new int[14];


    private void Start()
    {
        pos = player.GetComponent<Transform>();
        playerEntity = player.GetComponent<PlayerEntity>();
        LoadPlayerEntity();

        //this is for testing out of level ourder
        StartingHealth = playerEntity.MaxHealth;
        StartingMana = playerEntity.MaxMana;
        StartingSpeed = playerEntity.speed;
        StartingSize = playerEntity.cameraSize;
        StartingChargeable = new int[14];
        StartingJump = playerEntity.jumpForce;
    }

    private void Update()
    {
        pos = player.transform;
        if (playerEntity.Level == 1)
        {
            //main menu
        }
        if (pos.position.y < -53f && playerEntity.Level == 2) //complete first level
        {
            base.StartCoroutine(TransitionNextLevel());
        }



        if (playerEntity.isDead) //Player death 
        {
            if (playerEntity.Lives > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene " + playerEntity.Level, LoadSceneMode.Single);
                playerEntity.isDead = false;
                playerEntity.Lives--;
                playerEntity.MaxHealth = CheckpointHealth;
                playerEntity.MaxMana = CheckpointMana;
                playerEntity.cameraSize = CheckpointSize;
                playerEntity.speed = CheckpointSpeed;
                playerEntity.jumpForce = CheckpointJump;
                playerEntity.Chargeable = CheckpointChargeable;
                playerEntity.isClimbing = false;

                playerEntity.transform.position = playerEntity.CheckpointPos;
                SavePlayerEntity();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene " + playerEntity.Level, LoadSceneMode.Single);
                playerEntity.isDead = false;
                playerEntity.Lives = 3;
                playerEntity.MaxHealth = StartingHealth;
                playerEntity.MaxMana = StartingMana;
                playerEntity.cameraSize = StartingSize;
                playerEntity.speed = StartingSpeed;
                playerEntity.jumpForce = StartingJump;
                playerEntity.Chargeable = StartingChargeable;
                playerEntity.isClimbing = false;
                SavePlayerEntity();
            }
        }
    }

    public void StartNewSingleplayerGame()
    {
        playerEntity.Level = 1;
        base.StartCoroutine(TransitionNextLevel());
        playerEntity.MaxHealth = 100;
        playerEntity.MaxMana = 100;
        playerEntity.cameraSize = 3f;
        playerEntity.speed = 3f;
        playerEntity.jumpForce = 3.8f;
        playerEntity.Chargeable = new int[14];
        playerEntity.isClimbing = false;
        SavePlayerEntity();
    }
    public void ContinueSingleplayerGame()
    {
        playerEntity = LoadPlayerEntity();
        playerEntity.Level--;
        base.StartCoroutine(TransitionNextLevel());
    }

    private IEnumerator TransitionNextLevel(bool checkpoint)
    {
        transition.SetActive(true);
        SavePlayerEntity();

        yield return new WaitForSeconds(4f);//temp lowered
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene " + (playerEntity.Level + 1), LoadSceneMode.Single);
        playerEntity = LoadPlayerEntity();

        playerEntity.Lives++;
        playerEntity.Level++;
        StartingHealth = playerEntity.MaxHealth;
        StartingMana = playerEntity.MaxMana;
        StartingSize = playerEntity.cameraSize;
        StartingSpeed = playerEntity.speed;
        StartingJump = playerEntity.jumpForce;
        StartingChargeable = playerEntity.Chargeable;
        StartingHealth = playerEntity.MaxHealth;

        CheckpointHealth = playerEntity.MaxHealth;
        CheckpointMana = playerEntity.MaxMana;
        CheckpointSize = playerEntity.cameraSize;
        CheckpointSpeed = playerEntity.speed;
        CheckpointJump = playerEntity.jumpForce;
        CheckpointChargeable = playerEntity.Chargeable;
        CheckpointHealth = playerEntity.MaxHealth;

        playerEntity.isClimbing = false;

        if (checkpoint)
        {
            playerEntity.transform.position = playerEntity.CheckpointPos;
        }
        yield break;
    }
    
    public void SaveCheckPointValues()
    {
        CheckpointHealth = playerEntity.MaxHealth;
        CheckpointMana = playerEntity.MaxMana;
        CheckpointSize = playerEntity.cameraSize;
        CheckpointSpeed = playerEntity.speed;
        CheckpointJump = playerEntity.jumpForce;
        CheckpointChargeable = playerEntity.Chargeable;
        CheckpointHealth = playerEntity.MaxHealth;
    }
}
