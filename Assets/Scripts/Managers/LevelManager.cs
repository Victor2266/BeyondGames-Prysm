using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelToUnlock;

    public void WinLevel()
    {
        Debug.Log("Level: " + levelToUnlock + " completed");
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        if (levelReached <= levelToUnlock)
        {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        }
        MySceneManager.instance.SelectLevelScreen(true);
    }
}
