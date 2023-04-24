using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{

    public GameObject SceneManager;
    // Start is called before the first frame update
    void Start()
    {

        SceneManager = GameObject.FindGameObjectWithTag("SceneManager");
    }

    public void Singleplayer_startgame()
    {
        SceneManager.GetComponent<MySceneManager>().StartNewSingleplayerGame();
    }
}
