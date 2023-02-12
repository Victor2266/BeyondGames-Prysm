using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject PauseMenuUI, OptionsMenuUI, inventoryUI, ControlsMenuUI, keyboardControlsUI, controllerControlsUI;

    public GameObject player;
    public PlayerInput playerInput;
    public GameObject mousePointer;
    public GameObject MainMenuWarning;

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
        if (playerInput.actions["Pause"].WasPressedThisFrame())
        {
            //Debug.Log("Pausing Game");

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

        if (isPaused)
        {
            //check for controller press
            if (playerInput.actions["Navigate"].WasPressedThisFrame() && (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.active))
            {
                EventSystem.current.SetSelectedGameObject(null);
                GameObject defaultMenuOption = GameObject.FindGameObjectWithTag("DefaultOption");
                if (defaultMenuOption != null)
                    EventSystem.current.SetSelectedGameObject(defaultMenuOption);
            }
            else if (playerInput.actions["Point"].WasPerformedThisFrame())
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
    public void Resume()
    {
        playerInput.SwitchCurrentActionMap("Player");

        OptionsMenuUI.SetActive(false);
        inventoryUI.SetActive(false);
        MainMenuWarning.SetActive(false);
        ControlsMenuUI.SetActive(false);
        keyboardControlsUI.SetActive(false);
        controllerControlsUI.SetActive(false);

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
        playerInput.actions.FindActionMap("UI").Enable();
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
