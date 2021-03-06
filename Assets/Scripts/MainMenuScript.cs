﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer AudioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    public GameObject ObjectWithSceneManager;

    public GameObject MainMenu;
    public GameObject SingleplayerMenu;
    public GameObject MultiplayerMenu;

    public GameObject MainMenuWarning;

    private int depth;
    
    // Start is called before the first frame update
    void Start()
    {
        ObjectWithSceneManager.GetComponent<CameraController>().smoothTimeY = 2;

        ObjectWithSceneManager.GetComponent<CameraController>().offsetY = 1;
        depth = -1;


        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        Debug.Log(Screen.currentResolution);
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = resolutions.Length -1 - i;
            }
        }
        
        options.Reverse();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
    }


    // Update is called once per frame
    void Update()
    {
        if (depth == 0)
        {
            ObjectWithSceneManager.GetComponent<CameraController>().offsetX = 0;
            ObjectWithSceneManager.GetComponent<CameraController>().offsetY = 39;
            MainMenu.SetActive(true);
            SingleplayerMenu.SetActive(false);
            MultiplayerMenu.SetActive(false);
        }
    }
    public void Multiplayer()
    {
        ObjectWithSceneManager.GetComponent<CameraController>().smoothTimeY = 0.5f;
        ObjectWithSceneManager.GetComponent<CameraController>().offsetX = -490;
        ObjectWithSceneManager.GetComponent<CameraController>().offsetY = 9;
        MainMenu.SetActive(false);
        MultiplayerMenu.SetActive(true);
        depth = 1;
    }
    public void Singleplayer()
    {
        ObjectWithSceneManager.GetComponent<CameraController>().smoothTimeY = 0.5f;
        ObjectWithSceneManager.GetComponent<CameraController>().offsetX = 490;
        ObjectWithSceneManager.GetComponent<CameraController>().offsetY = 9;
        MainMenu.SetActive(false);
        SingleplayerMenu.SetActive(true);
        depth = 1;
    }
    public void Options()
    {
        ObjectWithSceneManager.GetComponent<CameraController>().smoothTimeY = 0.5f;
        ObjectWithSceneManager.GetComponent<CameraController>().offsetX = 0;
        ObjectWithSceneManager.GetComponent<CameraController>().offsetY = -430;
        //The options button activates the options menu
        //and the back button within the options menu deactivates the options menu
        MainMenu.SetActive(false);
        depth = 1;
    }
    public void SetVolume(float volume)
    {
        if (volume > 0)
        {
            AudioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }
        else
        {
            AudioMixer.SetFloat("Volume", -80f);
        }
        OptionsMenu.BGMVolume = volume;
    }
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutions.Length -1 - resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Singleplayer_startgame()
    {
        ObjectWithSceneManager.GetComponent<MySceneManager>().StartNewSingleplayerGame();
    }
    public void Back()
    {
        depth = 0;
    }
    public void QuitGame()
    {
        if (MainMenuWarning.activeSelf == false)
        {
            MainMenuWarning.SetActive(true);
        }
        else if (MainMenuWarning.activeSelf == true)
        {
            Application.Quit();
        }
    }
}
