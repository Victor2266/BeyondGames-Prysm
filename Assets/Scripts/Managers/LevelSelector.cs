using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public Toggle skipCutsceneToggle;
    private string skipToLevelName;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if(PlayerPrefs.GetInt("SkipCutscene", 0) == 1)
        {
            skipCutsceneToggle.isOn = true;
        }
        else
        {
            skipCutsceneToggle.isOn = false;
        }

        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].gameObject.SetActive(false);
            }
            
        }
    }
    public void Select (string levelName)
    {
        StartCoroutine(MySceneManager.instance.SelectLevel(levelName));
    }

    public void SetTargetLevel(string skipToLevelName)
    {
        this.skipToLevelName = skipToLevelName;
    }
    public void SelectLevel(string levelName)
    {
        if (PlayerPrefs.GetInt("SkipCutscene", 0) == 1)
        {
            StartCoroutine(MySceneManager.instance.SelectLevel(skipToLevelName));
            return;
        }
        StartCoroutine(MySceneManager.instance.SelectLevel(levelName));
    }

    public void setCutscenePref()
    {
        if (skipCutsceneToggle.isOn)
        {
            PlayerPrefs.SetInt("SkipCutscene", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SkipCutscene", 0);
        }
    }
}
