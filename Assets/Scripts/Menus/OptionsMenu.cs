using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    private Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreentoggle;
    public Toggle cameraLockToggle;
    public Toggle mouseVisibilityToggle;
    public Toggle weapIndicatorToggle;
    public Slider BGM_Slider;
    public Slider UI_Slider;
    public Slider CONTROL_Slider;

    // Callback which is triggered when
    // Camera lock setting is changed
    public delegate void OnCameraLockChanged();
    public static OnCameraLockChanged onCameraLockChangedCallback;
    // Callback which is triggered when
    // mosue show setting is changed
    public delegate void OnMouseVisibility();
    public static OnMouseVisibility onMouseVisibilityChangedCallback;
    // Callback which is triggered when
    // weap show setting is changed
    public delegate void OnWeapVisibility();
    public static OnWeapVisibility onWeapVisibilityChangedCallback;

    // Start is called before the first frame update
    void Start()
    {
        SetStartingSliders();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        //Debug.Log(Screen.currentResolution);
        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            string option = resolutions[i].ToString();
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = resolutions.Length - 1 - i;
            }
        }
        //options.Reverse();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullScreentoggle.isOn = Screen.fullScreen;
        cameraLockToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("CameraLock", 0) == 1);
        mouseVisibilityToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("mouseVisibility", 0) == 1);
        weapIndicatorToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("weapIndicator", 0) == 1);
    }

    private void SetStartingSliders()
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280f, 720f + 560f * PlayerPrefs.GetFloat("UISize", 0f));
        UI_Slider.value = PlayerPrefs.GetFloat("UISize", 0f);

        float volume = PlayerPrefs.GetFloat("BGM_Volume", 1f);
        BGM_Slider.value = volume;

        //clear delegate
        CONTROL_Slider.SetValueWithoutNotify(PlayerPrefs.GetFloat("controlSize", 0));

        if (volume > 0)
        {
            AudioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20f);
        }
        else
        {
            AudioMixer.SetFloat("Volume", -80f);
        }
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

    public void SetUiSize(float size)
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280 + 636 * size, 720);
        PlayerPrefs.SetFloat("UISize", size);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutions.Length - 1 - resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetCameraLock(bool isLocked) {
        if (isLocked)
        {
            PlayerPrefs.SetInt("CameraLock", 1);
        }
        else
        {
            PlayerPrefs.SetInt("CameraLock", 0);
        }

        onCameraLockChangedCallback.Invoke();
    }

    public void SetMouseVisibility(bool isVisible)
    {
        if (isVisible)
        {
            PlayerPrefs.SetInt("mouseVisibility", 1);
        }
        else
        {
            PlayerPrefs.SetInt("mouseVisibility", 0);
        }

        onMouseVisibilityChangedCallback.Invoke();
    }

    public void SetWeapIndicatorVisibility(bool isVisible)
    {
        if (isVisible)
        {
            PlayerPrefs.SetInt("weapIndicator", 1);
        }
        else
        {
            PlayerPrefs.SetInt("weapIndicator", 0);
        }

        onWeapVisibilityChangedCallback.Invoke();
    }

    public delegate void OnControlChange();
    public static event OnControlChange onControlChange;
    public void setControlSize(float size)
    {
        PlayerPrefs.SetFloat("controlSize", size);
        onControlChange.Invoke();
    }
}
