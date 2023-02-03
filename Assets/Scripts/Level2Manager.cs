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
    public GameObject blackBossExplosion;


    private void Start()
    {
        IKLS = ImperialKnight.GetComponent<ImperialKnightLongSword>();
    }
    public void Update()
    {
        if(index == 0)
        {
            if(IKLS.distToPlayer < 6f)
            {
                StartCoroutine(IKLSDelaySentence(1.5f, "Could there be an end to this?", 1f));
            }
        }
        else if(index == 2)
        {
            StartCoroutine(IKLSDelaySentence(2.5f, "What I'm feeling deep inside.", 1f));
        }
        else if(index == 4)
        {
            IKLS.ShowText(2f, "What is something like you doing here?", 1f);
            IKLS.agression = true;

            index++;
        }
        else if (index == 5)
        {
            if (IKLS.isDead)
            {
                blackBossExplosion.SetActive(true);
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

        if(IKLS.Health < IKLS.MaxHealth && index < 4)
        {
            index = 4;
        }
    }

    private IEnumerator IKLSDelaySentence(float WaitForAmount, string sentence, float size)
    {
        IKLS.ShowText(WaitForAmount, sentence, size);
        index++;//this stops the index in update loop
        yield return new WaitForSeconds(WaitForAmount);
        index++;//this moves onto next index if statement
    }
}
