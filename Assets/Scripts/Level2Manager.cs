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
    public GameObject knightHealthBar;
    public GameObject blackBossExplosion, hillPosObj, bossHealthBar;
    public DelayedCameraController cameraParent;
    public CircleCollider2D boss2headColider;

    private NewBoss2AI boss2AI;
    private Animator boss2Anim;
    public GameObject boss2;

    public Sprite DamagedKnightSprite;

    public ParticleSystem[] FOGEmitters;
    public Color PuppetSmoke, ShinigamiBlood;

    public SpriteRenderer scytheSprt;
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
            IKLS.ShowText(2f, "Something like you doesn't belong here,\nGo back to the forest or DIE.", 1f);
            IKLS.agression = true;
            knightHealthBar.SetActive(true);
            index++;
            KnightMusic.SetActive(true);
        }
        else if (index == 5)
        {
            if (IKLS.isDead)
            {
                StartCoroutine(IKLSDelaySentence(2.5f, "no way", 0.7f));
                ImperialKnight.GetComponent<SpriteRenderer>().sprite = DamagedKnightSprite;
                CameraShaker.Instance.ShakeOnce(15f, 5f, 0f, 2f);
                KnightMusic.SetActive(false);
            }
        }
        else if (index == 7)
        {
            CameraShaker.Instance.ShakeOnce(15f, 10f, 0f, 5f);
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
            StartCoroutine(ShinigamiDelaySentence(4f, "Now that I can add the hero of Giha to my collection\nI'll be unstoppable.", 1f));
        }
        else if (index == 14)
        {
            StartCoroutine(ShinigamiDelaySentence(4f, "It's time for me to seige the seaside kingdom.\n All there is to do now is get rid of a single <color=red>PEST</color>", 1f));
            boss2Anim.SetTrigger("Claws");
        }
        else if (index == 16)
        {
            cameraParent.TargetOverride = null;
            foreach(ParticleSystem p in FOGEmitters)
            {
                p.startColor = PuppetSmoke;
            }
            UndeadMusic.SetActive(true);
            boss2AI.ActivatePuppetWarrior();
            IKLS.ActivatePuppetMode();
            index++;
        }
        else if (index == 17)
        {
            if (IKLS.isDead)
            {
                boss2AI.DeactivatePuppetWarrior();
                StartCoroutine(ShinigamiDelaySentence(4f, "Goddamnit,\nYou just detroyed my new puppet", 1f));

                CameraShaker.Instance.ShakeOnce(15f, 10f, 0f, 2.5f);
                UndeadMusic.SetActive(false);
            }
        }
        else if (index == 19)
        {
            StartCoroutine(ShinigamiDelaySentence(2f, "If you want something DONE RIGHT\nyou have to do it yourself", 1f));
            boss2Anim.SetTrigger("TakeOutScythe");
            scytheSprt.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        }
        else if (index == 21)
        {
            StartCoroutine(ShinigamiDelaySentence(2f, "BEHOLD", 1f));
            ShinigamiMusic.SetActive(true);
        }
        else if (index == 23)
        {
            StartCoroutine(ShinigamiDelaySentence(4f, "HOW A TRUE SHINIGAMI CAN COMMAND LIFE AND <color=red>DEATH</color>", 1f));
            boss2headColider.enabled = true;
            bossHealthBar.SetActive(true);
            boss2AI.skeletonSpawner.SpawnAll();

            scytheSprt.maskInteraction = SpriteMaskInteraction.None;
        }
        else if (index == 25)
        {
            StartCoroutine(ShinigamiDelaySentence(4f, "<color=#6603fc><size=2>ARISE.", 1f));
            foreach (ParticleSystem p in FOGEmitters)
            {
                p.startColor = ShinigamiBlood;
            }

        }
        else if (index == 27)
        {
            boss2AI.agression = true;
            index++;
        }
        else if(index == 28)
        {
            if (boss2AI.isDead)
            {
                ShinigamiMusic.SetActive(false);
                bossSoul = GameObject.FindGameObjectWithTag("NoExplosion");
                StartCoroutine(ShinigamiDelaySentence(4f, "<color=red>I don't believe it . . .", 1f));
            }
        }

        else if (index == 30)
        {
            if (bossSoul == null)
            {
                deathInstructions.SetActive(false);
                CreditTextGameObject.SetActive(true);
                WinLevel();
                index++;//this is just to stop it from looping
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
