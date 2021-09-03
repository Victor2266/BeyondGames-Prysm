using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2BGManager : MonoBehaviour
{
    public GameObject CavernBG;
    public GameObject ForestBG;
    public GameObject BossBG;

    public GameObject Player;

    private void Awake()
    {
        CavernBG.SetActive(true);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Player.transform.position.y < 35f)
            {
                CavernBG.SetActive(false);
                ForestBG.SetActive(true);
            }
            else if (Player.transform.position.y > 35f)
            {
                CavernBG.SetActive(true);
                ForestBG.SetActive(false);
            }
        }
    }
}
