using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject ControlsMenuUI;
    public GameObject player;
    public PlayerInput playerInput;
    public GameObject mousePointer;
    public GameObject MainMenuWarning;
    public GameObject inventoryUI;

    public float testint;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["Pause"].WasPressedThisFrame() && inventoryUI.activeSelf == false)
        {
            Debug.Log("Pausing Game");
            OptionsMenuUI.SetActive(false);
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            TooltipManager.Hide();
        }
        else if (playerInput.actions["Pause"].WasPressedThisFrame() && inventoryUI.activeSelf == true)
        {
            Resume();
            inventoryUI.SetActive(false);

            TooltipManager.Hide();
        }
    }
    public void Resume()
    {
        MainMenuWarning.SetActive(false);
        ControlsMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        if(player.GetComponent<PlayerEntity>().weapon > 0)
        {
            mousePointer.SetActive(true);
        }
        else
        {
            mousePointer.SetActive(true);
        }
        player.GetComponent<PlayerController>().enabled = true;
        PauseMenuUI.SetActive(false);
        Time.timeScale = testint;
        isPaused = false;

    }
    public void Pause()
    {
        MainMenuWarning.SetActive(false);
        ControlsMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mousePointer.SetActive(false);
        player.GetComponent<PlayerController>().enabled = false;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Options()
    {

    }
    public void Saves()
    {

    }
    public void MainMenu()
    {
        if (MainMenuWarning.activeSelf == false)
        {
            MainMenuWarning.SetActive(true);
        }
        else if (MainMenuWarning.activeSelf == true)
        {
            player.GetComponent<PlayerEntity>().isDead = true;
            Resume();

            Cursor.visible = true;
        }
    }
}
