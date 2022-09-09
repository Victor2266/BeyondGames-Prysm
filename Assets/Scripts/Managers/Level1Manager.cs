using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : LevelManager// inherets winlevel function
{
    public BirdFollow birdFollower;
    public GameObject CheapSword;
    public int index = 0;

    public GameObject leftEnemyWave;
    public GameObject rightEnemyWave;
    public GameObject firstGoblin;

    public void Update()
    {
        //start of level, introduce bird follower
        if (index == 0)
        {
            StartCoroutine(DelaySentence(1f, "<color=yellow>SQUAK</color>", 1f));
        }
        else if (index == 2)
        {
            StartCoroutine(DelaySentence(1f, "<color=yellow>SQUAK</color>", 1f));
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
                index = 12;
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
                StartCoroutine(DelaySentence(5f, "great job", 1f));
            }
            else
            {
                index = 26;
            }
        }

        // when goblin is dead and soul is consumed, spawn goblin with spear to the left and 2 regulars goblins on right

        //when spear goblin dies, tell player to pick up their spear

        // when all the goblins are dead spawn spear researcher who complains about player invading theri territory 

        //when spear/re is dead, exposition about competition for souls & about lack of spear/re familiar

        //spawn lvl1 plant boss as familiar 

        if (index < 12 & EquipmentManager.instance.isEquipped(-1))
        {
            index = 22;
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
}
