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
    private ratBehavior firstGoblinScript;

    private ratBehavior spearGoblinScript;
    private ratBehavior Goblin1Script;
    private ratBehavior Goblin2Script;

    public GameObject spearGoblin;
    public GameObject goblin1;
    public GameObject goblin2;
    public GameObject spearResearcher;
    private Level1MiniBossModifiedGoblin spearResearcherScript;
    public GameObject Boss1;
    public GameObject Boss1Bar;
    public GameObject bossSoul;
    public GameObject BossRoomRange;
    public GameObject GoblinRoomRange;
    public GameObject SpearRoomRange;
    public Light globalLight;

    float ShakeMagnitude = 2f;
    [SerializeField]
    float ShakeRoughness = 3f;
    [SerializeField]
    float ShakeFadeIn = 0.5f;
    [SerializeField]
    float ShakeFadeOut = 8f;

    public SkillCheckDamage treeScript;
    public GameObject instructions;
    public GameObject CreditTextGameObject;
    private void Start()
    {
        firstGoblinScript = firstGoblin.GetComponent<ratBehavior>();

        spearGoblinScript = spearGoblin.GetComponent<ratBehavior>();
        Goblin1Script = goblin1.GetComponent<ratBehavior>();
        Goblin2Script = goblin2.GetComponent<ratBehavior>();

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
            //TreeFall();
        }else if (index == 24)
        {
            StartCoroutine(DelaySentence(5f, "oh wait, there's a log in the way.", 1f));
            ShowText(5f, "cut it down.", 1f);
            birdFollower.distance = 3.5f;
        }
        else if (index == 26)//tutorial 1
        {
            if (treeScript.isDead || firstGoblinScript.isDead)
            {
                TutorialPassed();
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "you gotta arc your swing more!", 1f));
            }
        }
        else if (index == 28)//tutorial 2
        {
            if (treeScript.isDead)
            {
                ShowText(3f, "FINALLY!", 1f);
                TutorialPassed();
            }
            else if (firstGoblinScript.isDead)
            {
                TutorialPassed();
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "bring the sword a bit closer and make a full circle!", 1f));
            }
        }
        else if (index == 30)//tutorial 3
        {
            if (treeScript.isDead)
            {
                ShowText(3f, "About time.", 1f);
                TutorialPassed();
            }
            else if (firstGoblinScript.isDead)
            {
                TutorialPassed();
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "it's not about speed, focus on the size of your swing", 1f));
            }
        }
        else if (index == 32)//tutorial 4
        {
            if (treeScript.isDead)
            {
                ShowText(3f, "that took a while,", 1f);
                TutorialPassed();
            }
            else if (firstGoblinScript.isDead)
            {
                TutorialPassed();
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "WIMP! you need to do at least <color=red>40 DMG!</color>", 1f));
            }
        }
        else if (index == 34)//tutorial repeat
        {
            StopAllCoroutines();
            index = 28;
        }
        else if(index == 23 || index == 25 || index == 27 || index == 29 || index == 31 || index == 33)
        {
            if (treeScript.isDead)
            {
                ShowText(3f, "finally,", 1f);
                TutorialPassed();
            }
        }
        else if (index == 24+14)
        {

            birdFollower.distance = 3f;
            StartCoroutine(DelaySentence(5f, "<color=red>ATTACK,</color> think of it as a mindless creature", 1f));
            ShowText(6f, "you can dash circles around it <color=green>[left shift]</color>", 1f);
        }

        //check if first golin has been killed
        //tell the benefits of collecting their soul
        else if (index == 26+14)
        {
            if (firstGoblinScript.isDead)
            {
                StartCoroutine(DelaySentence(2f, "great job!", 1f));
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "<color=red>KILL!</color>", 1f));
            }
        }
        else if (index == 28+14)
        {
            if (firstGoblinScript.isDead)
            {
                StartCoroutine(DelaySentence(2f, "excellent!", 1f));
            }
            else
            {
                index = 26+14;
            }
        }
        else if (index == 30+14)
        {
            StartCoroutine(DelaySentence(6f, "CONSUME its soul", 1f));
            ShowText(6f, "the more you consume the more <color=red>powerful</color> you will become", 1f);
        }

        // when goblin is dead and soul is consumed, spawn goblin with spear to the left and 2 regulars goblins on right
        else if (index == 32+14)
        {
            StartCoroutine(DelaySentence(6f, "Be careful, the goblins have been alerted of your presence", 1f));
            ShowText(6f, "get ready to <color=red>FIGHT</color>", 1f);
            leftEnemyWave.SetActive(true);
            rightEnemyWave.SetActive(true);
        }

        //when spear goblin dies, tell player to pick up their spear
        else if (index == 34+14)
        {
            if (spearGoblinScript.isDead)
            {
                StartCoroutine(DelaySentence(7f, "Nice! pickup that spear he dropped <color=green>[E]</color>", 1f));
                ShowText(7f, "when you run low on mana press <color=red>[Q]</color>", 1f);
            }
            if (spearGoblinScript.isDead && Goblin1Script.isDead && Goblin2Script.isDead)
            {
                GoblinRoomRange.SetActive(false);
            }
        }

        // when all the goblins are dead, spawn spear researcher who complains about player invading theri territory 

        else if (index == 36+14)
        {
            if (spearGoblinScript.isDead && Goblin1Script.isDead && Goblin2Script.isDead)
            {
                GoblinRoomRange.SetActive(false);
            }
            if (Goblin1Script.isDead && Goblin2Script.isDead)
            {
                SpearRoomRange.SetActive(true);
                StartCoroutine(DelaySentence(3f, "that can't be good", 1f));
                spearResearcher.SetActive(true);
                CameraShaker.Instance.ShakeOnce(ShakeMagnitude, ShakeRoughness, ShakeFadeIn, ShakeFadeOut);

                birdFollower.distance = 4.5f;
            }
        }
        else if (index == 38+14)
        {
            StartCoroutine(DelaySentence2(4f, "WHO ARE YOU?", 1f));
        }
        else if (index == 40+14)
        {
            StartCoroutine(DelaySentence2(7f, "THIS IS MY TERRITORY", 1f));
        }
        else if (index == 42+14)
        {
            StartCoroutine(DelaySentence2(7f, "THOSE ARE MY SOULS", 1f));
        }
        else if (index == 44+14)
        {
            StartCoroutine(DelaySentence2(2f, "<color=red>DIEEEE</color>", 3f));
        }
        //when spear/re is dead, exposition about competition for souls & about lack of spear/re familiar
        else if (index == 46+14)
        {
            if (spearResearcher.GetComponent<Level1MiniBossModifiedGoblin>().isDead)
            {
                SpearRoomRange.SetActive(false);
                birdFollower.distance = 3f;
                StartCoroutine(DelaySentence(7f, "thank god you beat him", 1f));
                ShowText(7f, "pick up the magic tome he dropped", 1f);
            }
        }
        else if (index == 48+14)
        {
            StartCoroutine(DelaySentence(7f, "we kinda did trespass on his hunting grounds", 1f));
            ShowText(7f, " and everyone is scrambling for souls right now", 1f);
        }
        else if (index == 50+14)
        {
            StartCoroutine(DelaySentence(6f, "because there's a rumour", 1f));
            ShowText(6f, "whoever collects enough souls can have one wish come true", 1f);
        }
        //spawn lvl1 plant boss as familiar 

        else if (index == 52+14)
        {
            StartCoroutine(DelaySentence(6f, "but it's strange...", 1f));
            ShowText(6f, "Chumps like that usually travel with a familiar...", 1f);

        }
        else if (index == 54+14)
        {
            StartCoroutine(DelaySentence(6f, "uh oh...", 1f));

            BossRoomRange.SetActive(true);
            birdFollower.distance = 4.5f;
            CameraShaker.Instance.ShakeOnce(ShakeMagnitude * 3f, ShakeRoughness * 2f, ShakeFadeIn, ShakeFadeOut * 3f);
            Boss1.SetActive(true);
            Boss1Bar.SetActive(true);
        }
        else if (index == 56+14)
        {
            if(Boss1.activeSelf == false)
            {
                Boss1Bar.SetActive(false);

                birdFollower.distance = 2f;
                StartCoroutine(DelaySentence(6f, "mission success", 1f));
                ShowText(6f, "collect that soul", 1f);

                BossRoomRange.SetActive(false);
                globalLight.intensity = 0f;
            }

        }
        else if (index == 58+14)
        {
            if(bossSoul == null)
            {
                instructions.SetActive(false);
                CreditTextGameObject.SetActive(true);
                WinLevel();
                index++;
            }
        }

        if (index < 12 && EquipmentManager.instance.isEquipped(-1))
        {
            index = 22;
            StopAllCoroutines();
        }
        else if (index >= 14 && index <= 24 && (treeScript.isDead || firstGoblinScript.isDead))
        {
            TutorialPassed();
        }
        else if(spearResearcherScript.isDead && index < 46+14 && index >= 36+14)
        {
            index = 46+14;
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
    public GameObject TreeHealthBar;
    private void TreeFall()
    {
        TreeBlockingPath.bodyType = RigidbodyType2D.Dynamic;
        TreeBlockingPath.mass = 10f;
        TreeBlockingPath.gameObject.tag = "box";
        Destroy(TreeHealthBar);
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

    private void TutorialPassed()
    {
        TreeFall();
        index = 36;
        StopAllCoroutines();
        if(!firstGoblinScript.isDead)
            StartCoroutine(DelaySentence(5f, "NOW, kill the goblin", 1f));
        else
        {
            index = 44;
            StartCoroutine(DelaySentence(5f, "BRO, there's an order to this shit", 1f));
        }
    }
}
