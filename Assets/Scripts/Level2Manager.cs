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
    public GameObject blackBossExplosion, hillPosObj;
    public DelayedCameraController cameraParent;

    private NewBoss2AI boss2AI;
    private Animator boss2Anim;
    public GameObject boss2;

    public Sprite DamagedKnightSprite;

    private void Start()
    {
        IKLS = ImperialKnight.GetComponent<ImperialKnightLongSword>();
        boss2AI = boss2.GetComponent<NewBoss2AI>();
        boss2Anim = boss2.GetComponent<Animator>();
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
                StartCoroutine(IKLSDelaySentence(2.5f, "no way", 0.7f));
                ImperialKnight.GetComponent<SpriteRenderer>().sprite = DamagedKnightSprite;
            }
        }
        else if (index == 7)
        {
            blackBossExplosion.SetActive(true);
            boss2.SetActive(true);
            cameraParent.TargetOverride = hillPosObj;
            index++;
        }
        else if (index == 8)
        {
            StartCoroutine(ShinigamiDelaySentence(3f, "", 1f));
        }
        else if (index == 10)
        {
            StartCoroutine(ShinigamiDelaySentence(1f, "Finally!\nHe's gone!", 1f));
        }
        else if (index == 12)
        {
            StartCoroutine(ShinigamiDelaySentence(4f, "Now that I can add the hero of catan to my collection\nI'll be unstoppable.", 1f));
        }
        else if (index == 14)
        {
            StartCoroutine(ShinigamiDelaySentence(4f, "It's time for me to seige the seaside kingdom.\n All there is to do now is get rid of a single <color=red>PEST</color>", 1f));
            boss2Anim.SetTrigger("Claws");
        }
        else if (index == 16)
        {
            cameraParent.TargetOverride = null;
            boss2AI.ActivatePuppetWarrior();
            IKLS.ActivatePuppetMode();
            index++;
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
            StopAllCoroutines();
        }
    }

    private IEnumerator IKLSDelaySentence(float WaitForAmount, string sentence, float size)
    {
        IKLS.ShowText(WaitForAmount, sentence, size);
        index++;//this stops the index in update loop
        yield return new WaitForSeconds(WaitForAmount);
        index++;//this moves onto next index if statement
    }
    private IEnumerator ShinigamiDelaySentence(float WaitForAmount, string sentence, float size)
    {
        boss2AI.ShowText(WaitForAmount, sentence, size);
        index++;//this stops the index in update loop
        yield return new WaitForSeconds(WaitForAmount);
        index++;//this moves onto next index if statement
    }


}
