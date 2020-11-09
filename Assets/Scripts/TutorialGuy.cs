using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuy : MonoBehaviour
{
    public GameObject TextPopUp;
    public GameObject Player;
    public GameObject GreenStar;

    private Animator anim;
    private Rigidbody2D rb2d;

    private float ConstVelo_x;

    private bool FacePlayer;

    private bool StartedSequence;
    private bool FinishedSquence;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), Player.GetComponent<CapsuleCollider2D>());
    }
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(ConstVelo_x, rb2d.velocity.y);

        if (FacePlayer)
        {
            if (Player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (StartedSequence == false && FinishedSquence == false)
            {
                StartedSequence = true;
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(StartingSequence());
            }
            else if (StartedSequence == true && FinishedSquence == false)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(BlastThroughWallSequence());
            }
            else if (StartedSequence == true && FinishedSquence == true)
            {
                ShowText("You should be ready to explore", 1.5f, Color.white, 3f, 0.05f);
            }
        }
    }

    private void ShowText(string text, float size, Color colour, float timeLength, float teletypeSpeed)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.25f, -1f);
        GameObject gameObject2 = Instantiate(TextPopUp, position, transform.rotation);

        gameObject2.GetComponent<TMPro.Examples.TeleType>().DisplayText = text;
        gameObject2.GetComponent<TMPro.Examples.TeleType>().delay = teletypeSpeed;
        gameObject2.GetComponent<TMPro.Examples.TeleType>().RefreshDisplayText();

        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<TMPro.TextMeshPro>().color = colour;

        gameObject2.GetComponent<DeathTimer>().tickLimit = timeLength;
        gameObject2.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0.1f, 0f);

        gameObject2.GetComponent<TMPro.Examples.TeleType>().StartCoroutine(gameObject2.GetComponent<TMPro.Examples.TeleType>().StartText());
    }
    IEnumerator StartingSequence()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);

        ShowText("Oh?", 1.5f, Color.white, 2f, 0.1f);
        yield return new WaitForSeconds(2f);

        ShowText("a traveller?", 1.5f, Color.white, 3f, 0.1f);
        yield return new WaitForSeconds(3f);

        ShowText("You won't last very long like that", 1.5f, Color.white, 6f, 0.05f);
        yield return new WaitForSeconds(5f);

        ShowText("Follow me.", 2f, Color.white, 3f, 0.1f);
        yield return new WaitForSeconds(2f);

        anim.SetBool("Running", true);
        ConstVelo_x = 2f;
        yield return new WaitForSeconds(8f);

        anim.SetBool("Running", false);
        ConstVelo_x = 0f;
        FacePlayer = true;
        yield return new WaitForSeconds(3f);

        GetComponent<BoxCollider2D>().enabled = true;
    }
    IEnumerator BlastThroughWallSequence()
    {
        ShowText("Watch this.", 1.5f, Color.white, 3f, 0.1f);
        yield return new WaitForSeconds(2f);


        Vector3 position = new Vector3(transform.position.x + 0.5f, transform.position.y + 1.25f, -1f);
        GameObject greenStar = Instantiate(GreenStar, position, transform.rotation);
        yield return new WaitForSeconds(5f);

        greenStar.GetComponent<Rigidbody2D>().velocity = new Vector2(-2f, 0f);
        yield return new WaitForSeconds(3f);

        ShowText("Go pick Up that Book", 1.5f, Color.white, 3f, 0.1f);

        FinishedSquence = true;
    }
}
