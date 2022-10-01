using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelToUnlock;

    public void WinLevel()
    {
        Debug.Log("Level: " + (levelToUnlock-1) + " completed");
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        if (levelReached <= levelToUnlock)
        {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        }
        StartCoroutine(MySceneManager.instance.SelectLevelScreen(true));//this automatically finds the transition and activates it

    }
}
