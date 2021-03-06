﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject player;
    public GameObject mousePointer;
    public GameObject MainMenuWarning;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            OptionsMenuUI.SetActive(false);
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        MainMenuWarning.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        mousePointer.SetActive(true);
        player.GetComponent<PlayerController>().enabled = true;
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }
    void Pause()
    {
        MainMenuWarning.SetActive(false);
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
            player.GetComponent<PlayerEntity>().Lives = 0;
            player.GetComponent<PlayerEntity>().isDead = true;
            Resume();

            Cursor.visible = true;
        }
    }
}
