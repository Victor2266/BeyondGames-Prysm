using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Level2Manager : LevelManager// inherets winlevel function
{
    public int index = 0;
    public GameObject player;

    float ShakeMagnitude = 2f;
    [SerializeField]
    float ShakeRoughness = 3f;
    [SerializeField]
    float ShakeFadeIn = 0.5f;
    [SerializeField]
    float ShakeFadeOut = 8f;

    public GameObject bossSoul;
    public GameObject deathInstructions;
    public GameObject KnightMusic, UndeadMusic, ShinigamiMusic;
    public GameObject ImperialKnight;
    private ImperialKnightLongSword IKLS;


    private void Start()
    {
        IKLS = ImperialKnight.GetComponent<ImperialKnightLongSword>();
    }
    public void Update()
    {
        if(index == 0)
        {
            if(IKLS.distToPlayer < 8)
            {
                IKLS.agression = true;
            }
        }
        if (index == 100)
        {
            if (bossSoul == null)
            {
                deathInstructions.SetActive(false);
                CreditTextGameObject.SetActive(true);
                WinLevel();
                index++;
            }
        }
    }
}
