using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Level2Manager : LevelManager// inherets winlevel function
{
    public int index = 0;
    public GameObject player;

    private bool completedLevel = false;
    float ShakeMagnitude = 2f;
    [SerializeField]
    float ShakeRoughness = 3f;
    [SerializeField]
    float ShakeFadeIn = 0.5f;
    [SerializeField]
    float ShakeFadeOut = 8f;

    private void Start()
    {

    }
    public void Update()
    {
        if(player.transform.position.x < -38.4f && !completedLevel)
        {
            completedLevel = true;
            player.SendMessage("TakeDamage", 100);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(15f, 15f);
            WinLevel();
        }
    }
}
