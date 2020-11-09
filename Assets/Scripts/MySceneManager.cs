using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private void Start()
    {
        pos = player.GetComponent<Transform>();
        //this is for testing out of level ourder
        StartingHealth = PlayerController.MaxHealth;
        StartingMana = PlayerController.MaxMana;
        StartingSpeed = PlayerController.speed;
        StartingSize = PlayerController.cameraSize;
        StartingChargeable = new int[14];
        StartingJump = PlayerController.jumpForce;
    }

    private void Update()
    {
        pos = player.transform;
        if (Input.anyKeyDown && !Input.GetMouseButton(0) && Level == 1)
        {
            
        }
        if (pos.position.y < -53f && Level == 2)
        {
            base.StartCoroutine(Trans());
        }
        if (PlayerController.isDead)
        {
            if (PlayerController.Lives > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene " + Level, LoadSceneMode.Single);
                PlayerController.isDead = false;
                PlayerController.Lives--;
                PlayerController.MaxHealth = StartingHealth;
                PlayerController.MaxMana = StartingMana;
                PlayerController.cameraSize = StartingSize;
                PlayerController.speed = StartingSpeed;
                PlayerController.jumpForce = StartingJump;
                PlayerController.Chargeable = StartingChargeable;
                PlayerController.isClimbing = false;
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene " + 1, LoadSceneMode.Single);
                PlayerController.isDead = false;
                Level = 1;
            }
        }
    }
    public void StartSingleplayerGame()
    {
        base.StartCoroutine(Trans());
        PlayerController.MaxHealth = 100;
        PlayerController.MaxMana = 100;
        PlayerController.cameraSize = 3f;
        PlayerController.speed = 3f;
        PlayerController.jumpForce = 3.8f;
        PlayerController.Chargeable = new int[14];
        PlayerController.isClimbing = false;
    }
    private IEnumerator Trans()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(4f);//temp lowered
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene " + (Level + 1), LoadSceneMode.Single);
        PlayerController.Lives++;
        Level++;
        StartingHealth = PlayerController.MaxHealth;
        StartingMana = PlayerController.MaxMana;
        StartingSize = PlayerController.cameraSize;
        StartingSpeed = PlayerController.speed;
        StartingJump = PlayerController.jumpForce;
        StartingChargeable = PlayerController.Chargeable;
        StartingHealth = PlayerController.MaxHealth;
        PlayerController.isClimbing = false;
        yield break;
    }
    

    public GameObject player;

    private Transform pos;

    public GameObject transition;

    public static int Level = 1;

    public static int StartingHealth;

    public static int StartingMana;

    private static float StartingSpeed;

    private static float StartingSize;

    public static float StartingJump;

    private static int[] StartingChargeable = new int[14];
}
