using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public int levelToUnlock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
