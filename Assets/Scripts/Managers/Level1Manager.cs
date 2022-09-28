using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Level1Manager : LevelManager// inherets winlevel function
{
    public BirdFollow birdFollower;
    public GameObject CheapSword;
    public int index = 0;

    public GameObject leftEnemyWave;
    public GameObject rightEnemyWave;
    public GameObject firstGoblin;
    public GameObject spearGoblin;
    public GameObject goblin1;
    public GameObject goblin2;
    public GameObject spearResearcher;
    private Level1MiniBossModifiedGoblin spearResearcherScript;
    public GameObject Boss1;
    public GameObject Boss1Bar;

    float ShakeMagnitude = 2f;
    [SerializeField]
    float ShakeRoughness = 3f;
    [SerializeField]
    float ShakeFadeIn = 0.5f;
    [SerializeField]
    float ShakeFadeOut = 8f;

    private void Start()
    {
        spearResearcherScript = spearResearcher.GetComponent<Level1MiniBossModifiedGoblin>();
    }
    public void Update()
    {
        //start of level, introduce bird follower
        if (index == 0)
        {
            StartCoroutine(DelaySentence(2f, "<color=yellow>SQUAK</color>", 1f));
        }
        else if (index == 2)
        {
            StartCoroutine(DelaySentence(2f, "<color=yellow>SQUAK</color>", 1f));
        }
        else if(index == 4)
        {
            StartCoroutine(DelaySentence(2f, "S-STAY PUT", 1f));
        }
        else if(index == 6)
        {
            StartCoroutine(DelaySentence(4f, "I was sent to guide you", 1f));
        }
        else if(index == 8)
        {
            StartCoroutine(DelaySentence(6f, "by <color=#5888FF>Renka</color>, your benefactor", 1f));
        }

        //teach how to equip sword and inventory delete floating key cap
        else if (index == 10)
        {
            // check if sword is picked up yet, if it is then skip forwards
            if (CheapSword != null)
            {
                StartCoroutine(DelaySentence(6f, "pick up that sword on the LEFT <color=green>(press [E])</color>", 1f));
            }
            else
            {
                index = 12;
            }
        }
        else if (index == 12)
        {
            if (CheapSword != null)
            {
                index = 10;
            }
            else
            {
                StartCoroutine(DelaySentence(6f, "equip the sword in your inventory <color=green>[I]</color>", 1f));
            }
        }
        else if (index == 14)
        {
            StartCoroutine(DelaySentence(3.5f, "try swinging it around <color=green>[left click]</color>", 1f));
        }
        else if (index == 16)
        {
            StartCoroutine(DelaySentence(3.5f, "the larger the swing the more damage!", 1f));
        }
        else if (index == 18)
        {
            StartCoroutine(DelaySentence(3.5f, "and each weapon has a special <color=green>[right click]</color> move", 1f));
        }
        //tell to walk over to right to fight goblin
        else if (index == 20)
        {
            StartCoroutine(DelaySentence(2.5f, "after you're done warming up", 1f));
        }
        else if (index == 22)//skip to here if they equip the sword early
        {
            StartCoroutine(DelaySentence(5f, "<color=red>ATTACK</color> the goblin to your right", 1f));
            birdFollower.distance = 3f;
            TreeFall();
        }
        else if (index == 24)
        {
            StartCoroutine(DelaySentence(5f, "think of it as a mindless creature", 1f));
            ShowText(6f, "you can dash circles around it <color=green>[left shift]</color>", 1f);
        }

        //check if first golin has been killed
        //tell the benefits of collecting their soul
        else if (index == 26)
        {
            if (firstGoblin.GetComponent<ratBehavior>().isDead)
            {
                StartCoroutine(DelaySentence(5f, "great job!", 1f));
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "<color=red>KILL!</color>", 1f));
            }
        }
        else if (index == 28)
        {
            if (firstGoblin.GetComponent<ratBehavior>().isDead)
            {
                StartCoroutine(DelaySentence(5f, "excellent!", 1f));
            }
            else
            {
                index = 26;
            }
        }
        else if (index == 30)
        {
            StartCoroutine(DelaySentence(6f, "CONSUME its soul", 1f));
            ShowText(6f, "the more you consume the more <color=red>powerful</color> you will become", 1f);
        }

        // when goblin is dead and soul is consumed, spawn goblin with spear to the left and 2 regulars goblins on right
        else if (index == 32)
        {
            StartCoroutine(DelaySentence(6f, "Be careful, the goblins have been alerted of your presence", 1f));
            ShowText(6f, "get ready to <color=red>FIGHT</color>", 1f);
            leftEnemyWave.SetActive(true);
            rightEnemyWave.SetActive(true);
        }

        //when spear goblin dies, tell player to pick up their spear
        else if (index == 34)
        {
            if (spearGoblin.GetComponent<ratBehavior>().isDead)
            {
                StartCoroutine(DelaySentence(7f, "Nice! pickup that spear he dropped <color=green>[E]</color>", 1f));
            }
        }

        // when all the goblins are dead, spawn spear researcher who complains about player invading theri territory 

        else if (index == 36)
        {
            if (goblin1.GetComponent<ratBehavior>().isDead && goblin2.GetComponent<ratBehavior>().isDead)
            {
                StartCoroutine(DelaySentence(3f, "that can't be good", 1f));
                spearResearcher.SetActive(true);
                CameraShaker.Instance.ShakeOnce(ShakeMagnitude, ShakeRoughness, ShakeFadeIn, ShakeFadeOut);
            }
        }
        else if (index == 38)
        {
            StartCoroutine(DelaySentence2(4f, "WHO ARE YOU?", 1f));
        }
        else if (index == 40)
        {
            StartCoroutine(DelaySentence2(7f, "THIS IS MY TERRITORY", 1f));
        }
        else if (index == 42)
        {
            StartCoroutine(DelaySentence2(7f, "THOSE ARE MY SOULS", 1f));
        }
        else if (index == 44)
        {
            StartCoroutine(DelaySentence2(6f, "<color=red>DIEEEE</color>", 3f));
        }
        //when spear/re is dead, exposition about competition for souls & about lack of spear/re familiar
        else if (index == 46)
        {
            if (spearResearcher.GetComponent<Level1MiniBossModifiedGoblin>().isDead)
            {
                StartCoroutine(DelaySentence(7f, "thank god you beat him", 1f));
                ShowText(7f, "pick up, equip, and try using the magic tome he dropped", 1f);
            }
        }
        else if (index == 48)
        {
            StartCoroutine(DelaySentence(7f, "we sort of trespassed on his hunting grounds", 1f));
            ShowText(7f, " and everyone is scrambling for souls right now", 1f);
        }
        else if (index == 50)
        {
            StartCoroutine(DelaySentence(6f, "because there's a rumour", 1f));
            ShowText(6f, "whoever collects enough souls can have their dreams come true", 1f);
        }
        //spawn lvl1 plant boss as familiar 

        else if (index == 52)
        {
            StartCoroutine(DelaySentence(6f, "but it's strange...", 1f));
            ShowText(6f, "Shinigami usually travel with a familiar...", 1f);

        }
        else if (index == 54)
        {
            StartCoroutine(DelaySentence(6f, "uh oh...", 1f));
            CameraShaker.Instance.ShakeOnce(ShakeMagnitude * 2f, ShakeRoughness * 2f, ShakeFadeIn, ShakeFadeOut);
            Boss1.SetActive(true);
            Boss1Bar.SetActive(true);

        }
        else if (index == 56)
        {
            if(Boss1.activeSelf == false)
            {
                Boss1Bar.SetActive(false);

                StartCoroutine(DelaySentence(6f, "mission success", 1f));
            }

        }
        else if (index == 58)
        {
            WinLevel();
        }

        if (index < 12 & EquipmentManager.instance.isEquipped(-1))
        {
            index = 22;
            StopAllCoroutines();
        }
        else if(spearResearcherScript.isDead && index < 46 && index >= 36)
        {
            index = 46;
            StopAllCoroutines();
        }
    }

    public GameObject TextObject;
    private GameObject nextMsg;
    private GameObject thisMsg = null;
    public void ShowText(float waitForAmount, string txt, float size)
    {
        if (nextMsg != null)
        {
            thisMsg = nextMsg;
        }
        nextMsg = Instantiate(TextObject, birdFollower.transform);
        nextMsg.GetComponent<InGameTextMessage>().lastMSG = thisMsg;

        if (thisMsg != null)
        {
            nextMsg.GetComponent<InGameTextMessage>().moveLastMSG();
            //lastMsg.transform.localPosition = new Vector3(0f, 0.4f, 0f);
            //lastMsg.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 1.4f, 0f);
        }
        nextMsg.transform.localPosition = new Vector3(0f, 0.2f, 0f);
        nextMsg.GetComponent<TMPro.TextMeshPro>().text = txt;
        nextMsg.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        nextMsg.GetComponent<DeathTimer>().tickLimit = waitForAmount*1.1f;
        //NetworkServer.Spawn(gameObject2);
    }

    private IEnumerator DelaySentence(float WaitForAmount, string sentence, float size)
    {
        ShowText(WaitForAmount, sentence, size);
        index++;
        yield return new WaitForSeconds(WaitForAmount);
        index++;
    }

    public Rigidbody2D TreeBlockingPath;
    private void TreeFall()
    {
        TreeBlockingPath.bodyType = RigidbodyType2D.Dynamic;
        TreeBlockingPath.mass = 10f;
    }

    public GameObject TextObject2;
    private IEnumerator DelaySentence2(float WaitForAmount, string sentence, float size)
    {
        ShowResearcherText(WaitForAmount, sentence, size);
        index++;
        yield return new WaitForSeconds(WaitForAmount);
        index++;
    }
    public void ShowResearcherText(float waitForAmount, string txt, float size)
    {
        if (nextMsg != null)
        {
            thisMsg = nextMsg;
        }
        nextMsg = Instantiate(TextObject2, spearResearcher.transform);
        nextMsg.GetComponent<InGameTextMessage>().lastMSG = thisMsg;

        if (thisMsg != null)
        {
            nextMsg.GetComponent<InGameTextMessage>().moveLastMSG();
            //lastMsg.transform.localPosition = new Vector3(0f, 0.4f, 0f);
            //lastMsg.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 1.4f, 0f);
        }
        nextMsg.transform.localPosition = new Vector3(0f, 0.2f, 0f);
        nextMsg.GetComponent<TMPro.TextMeshPro>().text = txt;
        nextMsg.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        nextMsg.GetComponent<DeathTimer>().tickLimit = waitForAmount * 1.1f;
        //NetworkServer.Spawn(gameObject2);
    }
}
