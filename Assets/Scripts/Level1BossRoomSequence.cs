using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1BossRoomSequence : MonoBehaviour
{
    public GameObject TextPopUp;
    public GameObject Player;
    public GameObject GreenPortal;
    public GameObject Researcher;
    public GameObject Boss1;
    public GameObject BossRoomRange;

    private Animator anim;
    private Rigidbody2D rb2d;

    private bool StartedSequence;
    public bool FinishedSequence;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        if (Researcher.GetComponent<Level1MiniBossModifiedGoblin>().isDead == true)
        {
            FinishedSequence = true;
            ShowText("What have you done . . .", 2f, Color.red, 3f, 0.05f);
        }
        if (FinishedSequence)
        {
            Boss1.SetActive(true);
            BossRoomRange.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (StartedSequence == false)
            {
                StartedSequence = true;
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(StartingSequence());
            }
        }
    }

    private void ShowText(string text, float size, Color colour, float timeLength, float teletypeSpeed)
    {
        Vector3 position = new Vector3(Researcher.transform.position.x, Researcher.transform.position.y + 0.4f, Researcher.transform.position.z);
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
        GreenPortal.SetActive(true);
        yield return new WaitForSeconds(3f);
        
        Researcher.SetActive(true);
        Researcher.GetComponent<Level1MiniBossModifiedGoblin>().aggression = false;
        yield return new WaitForSeconds(2f);
        
        Researcher.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        ShowText("Who are you?", 1.5f, Color.red, 4f, 0.05f);
        yield return new WaitForSeconds(5f);

        ShowText("You shouldn't be here.", 2.5f, Color.red, 5f, 0.05f);
        yield return new WaitForSeconds(3f);

        Researcher.GetComponent<Level1MiniBossModifiedGoblin>().aggression = true;

        Researcher.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
