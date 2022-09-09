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
            StartCoroutine(DelaySentence(0.5f, "SQUAK", 2f));
        }
        else if (index == 2)
        {
            StartCoroutine(DelaySentence(0.5f, "SQUAK", 2f));
        }
        else if(index == 4)
        {
            StartCoroutine(DelaySentence(1.5f, "S-STAY PUT", 2f));
        }
        else if(index == 6)
        {
            StartCoroutine(DelaySentence(5f, "heyo im your benefactor", 2f));
        }
        else if(index == 8)
        {
            StartCoroutine(DelaySentence(6f, "I possessed this crow to guide you", 2f));
        }

        //teach how to equip sword and inventory delete floating key cap
        else if (index == 10)
        {
            // check if sword is picked up yet, if it is then skip forwards
            if (CheapSword != null)
            {
                StartCoroutine(DelaySentence(7f, "pick up that sword on the LEFT (press [E])", 2f));
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
                index = 8;
            }
            StartCoroutine(DelaySentence(7f, "equip the sword in your inventory [I]", 2f));
        }
        else if (index == 14)
        {
            StartCoroutine(DelaySentence(5f, "try swinging it around", 2f));
        }
        else if (index == 16)
        {
            StartCoroutine(DelaySentence(6f, "the larger the swing the more damage!", 2f));
        }
        else if (index == 18)
        {
            StartCoroutine(DelaySentence(5f, "and each weapon has a special [right click] move", 2f));
        }
        //tell to walk over to right to fight goblin
        else if (index == 20)
        {
            StartCoroutine(DelaySentence(5f, "after you're done warming up", 2f));
        }
        else if (index == 22)
        {
            StartCoroutine(DelaySentence(5f, "ATTACK the goblin to your right", 2f));
        }
        else if (index == 24)
        {
            StartCoroutine(DelaySentence(5f, "just think of them as unthinking and unfeeling monster", 2f));
        }

        //check if first golin has been killed
        //tell the benefits of collecting their soul
        else if (index == 26)
        {
            if (firstGoblin.GetComponent<ratBehavior>().isDead)
            {
                StartCoroutine(DelaySentence(5f, "great job!", 2f));
            }
            else
            {
                StartCoroutine(DelaySentence(5f, "<color=red>KILL!</color>", 2f));
            }
        }
        else if (index == 28)
        {
            if (firstGoblin.GetComponent<ratBehavior>().isDead)
            {
                StartCoroutine(DelaySentence(5f, "NEXT", 2f));
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
    }

    public GameObject TextObject;
    private GameObject nextMsg;
    private GameObject thisMsg = null;
    public void ShowText(string txt, float size)
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
        //thisMsg.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        nextMsg.GetComponent<DeathTimer>().tickLimit = txt.Length / 6f;
        //NetworkServer.Spawn(gameObject2);
    }

    private IEnumerator DelaySentence(float WaitForAmount, string sentence, float size)
    {
        ShowText(sentence, size);
        index++;
        yield return new WaitForSeconds(WaitForAmount);
        index++;
    }

}
