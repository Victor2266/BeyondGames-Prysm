using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public int DMG;

    private Rigidbody2D rb2d;
    private GameObject player;

    public bool Bouncy;
    public GameObject pop;
    public GameObject InitialPop = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        
        Vector3 popPosition = new Vector3(transform.position.x, transform.position.y + .05f, InitialPop.transform.position.z);
        if (InitialPop != null)
        {
            Instantiate(InitialPop, popPosition, transform.rotation);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            Burst();
        }

        if (collision.gameObject.tag == "Player")
        {
            player.SendMessage("TakeDamage", DMG);
            Burst();
        }
        //if (collision.gameObject.tag == "enemy")
        //{
            //collision.gameObject.GetComponent<MobGeneric>().SendMessage("TakeDamage", DMG);
            //Burst();
        //}
        if (Bouncy == false && collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Ground")
        {
            Burst();
        }
    }
  
    private void Burst()
    {
        //this.gameObject.SetActive(false);
        GameObject gameObject = Instantiate(pop, transform.position, transform.rotation);

        Destroy(this.gameObject);

    }

}