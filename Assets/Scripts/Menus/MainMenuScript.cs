using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer AudioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreentoggle;
    public Toggle cameraLockToggle;

    public GameObject MainCamera;
    public GameObject SceneManager;

    public GameObject MainMenu;
    public GameObject SingleplayerMenu;

    public GameObject MultiplayerMenu;

    private int depth;

    private Vector2 velocity;
    private float cameraTargetSize = 22.23f;

    public PlayerInput playerInput;

    public Slider BGM_Slider;
    public Slider UI_Slider;
    public Slider CONTROL_Slider;
    // Start is called before the first frame update
    void Start()
    {
        SetStartingSliders();

        MainCamera.GetComponent<OldCameraController>().smoothTimeY = 2;
        SceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        MainCamera.GetComponent<OldCameraController>().offsetY = 50.5f;
        depth = -1;


        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        Debug.Log(Screen.currentResolution);
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].ToString();
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = resolutions.Length -1 - i;
            }
        }
        
        options.Reverse();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullScreentoggle.isOn = Screen.fullScreen;
        cameraLockToggle.isOn = PlayerPrefs.GetInt("CameraLock", 0) == 1 ? true : false;
    }

    private void SetStartingSliders()
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720 + 560 * PlayerPrefs.GetFloat("UISize", 0f));
        UI_Slider.value = PlayerPrefs.GetFloat("UISize", 0f);

        float volume = PlayerPrefs.GetFloat("BGM_Volume", 1f);
        BGM_Slider.value = volume;

        CONTROL_Slider.value = PlayerPrefs.GetFloat("controlSize", 0);

        if (volume > 0)
        {
            AudioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }
        else
        {
            AudioMixer.SetFloat("Volume", -80f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (depth == 0)
        {
            MainCamera.GetComponent<OldCameraController>().offsetX = 0;
            MainCamera.GetComponent<OldCameraController>().offsetY = 50.5f;
            MainMenu.SetActive(true);
            SingleplayerMenu.SetActive(false);
            MultiplayerMenu.SetActive(false);
            depth = -1;

        }

        //check for controller press
        if (playerInput.actions["Navigate"].WasPressedThisFrame() && (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.active))
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("DefaultOption"));
        }
        else if (playerInput.actions["Point"].WasPerformedThisFrame())
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        float num = Mathf.SmoothDamp(MainCamera.GetComponent<Camera>().orthographicSize, cameraTargetSize, ref velocity.y, 1f);
        MainCamera.GetComponent<Camera>().orthographicSize = num;
    }
    public void Multiplayer()
    {
        MainCamera.GetComponent<OldCameraController>().smoothTimeY = 0.5f;
        MainCamera.GetComponent<OldCameraController>().offsetX = -490;
        MainCamera.GetComponent<OldCameraController>().offsetY = 24f;
        MainMenu.SetActive(false);
        MultiplayerMenu.SetActive(true);
        depth = 1;
    }
    public void Singleplayer()
    {
        MainCamera.GetComponent<OldCameraController>().smoothTimeY = 0.5f;
        MainCamera.GetComponent<OldCameraController>().offsetX = 490;
        MainCamera.GetComponent<OldCameraController>().offsetY = 21.5f;
        MainMenu.SetActive(false);
        SingleplayerMenu.SetActive(true);
        depth = 1;

        cameraTargetSize = 4.2f;
    }
    public void Options()
    {
        MainCamera.GetComponent<OldCameraController>().smoothTimeY = 0.5f;
        MainCamera.GetComponent<OldCameraController>().offsetX = 0;
        MainCamera.GetComponent<OldCameraController>().offsetY = -430;
        //The options button activates the options menu
        //and the back button within the options menu deactivates the options menu
        MainMenu.SetActive(false);
        depth = 1;
    }
    public void SupportLink()
    {
        Application.OpenURL("https://www.patreon.com/awasete");
        Application.OpenURL("https://www.kickstarter.com/projects/awasete/beyond-awasete");
        Application.OpenURL("https://linktr.ee/awasete");
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
        PlayerPrefs.SetFloat("BGM_Volume", volume);
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
        SceneManager.GetComponent<MySceneManager>().StartNewSingleplayerGame();
    }
    public void Singleplayer_continuegame()
    {
        SceneManager.GetComponent<MySceneManager>().ContinueSingleplayerGame();
    }
    public void Back()
    {
        depth = 0;
        cameraTargetSize = 22.23f;
    }
    public void QuitGame()
    {
         Application.Quit();

    }
    public void SetCameraLock(bool isLocked)
    {
        if (isLocked)
        {
            PlayerPrefs.SetInt("CameraLock", 1);
        }
        else
        {
            PlayerPrefs.SetInt("CameraLock", 0);
        }
    }

    public void SetUiSize(float size)
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720 + 560 * size);
        PlayerPrefs.SetFloat("UISize", size);
    }

    public void setControlSize(float size)
    {
        PlayerPrefs.SetFloat("controlSize", size);
    }
}
