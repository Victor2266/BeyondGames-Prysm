using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : LevelManager// inherets winlevel function
{
    public BirdFollow birdFollower;
    public GameObject CheapSword;
    public int index = 0;

    public void Update()
    {
        //start of level, introduce bird follower
        if (index == 0)
        {
            StartCoroutine(DelaySentence(1f, "SQUAK", 2f));
        }
        else if (index == 2)
        {
            StartCoroutine(DelaySentence(1f, "SQUAK", 2f));
        }
        else if(index == 4)
        {
            StartCoroutine(DelaySentence(3f, "S-STAY PUT", 2f));
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

        //tell the benefits of collecting their soul

        // when goblin is dead and soul is consumed, spawn goblin with spear to the left and 2 regulars goblins on right

        //when spear goblin dies, tell player to pick up their spear

        // when all the goblins are dead spawn spear researcher who complains about player invading theri territory 

        //when spear/re is dead, exposition about competition for souls & about lack of spear/re familiar

        //spawn lvl1 plant boss as familiar 
    }

    public GameObject TextObject;
    public void ShowText(string txt, float size)
    {
        GameObject gameObject2 = Instantiate(TextObject, birdFollower.transform);
        gameObject2.transform.localPosition = new Vector3(0f, 0.2f, 0f);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = txt;
        //gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<DeathTimer>().tickLimit = txt.Length / 6f;
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
