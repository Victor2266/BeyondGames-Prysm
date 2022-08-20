using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    private void Start()
    {
        this.rb2d = base.GetComponent<Rigidbody2D>();
        if (MoveOnStart == true)
        {
            if (base.transform.position.y == this.TopPos.y)
            {
                this.GoBot();
            }
            if (base.transform.position.y == this.BotPos.y)
            {
                this.GoTop();
            }
        }
    }

    private void Update()
    {
        if (this.rb2d.position.y > this.TopPos.y)
        {
            this.rb2d.velocity = new Vector2(0f, 0f);
            this.rb2d.position = new Vector2(base.transform.position.x, this.TopPos.y);
        }
        if (this.rb2d.position.y < this.BotPos.y)
        {
            this.rb2d.velocity = new Vector2(0f, 0f);
            this.rb2d.position = new Vector2(base.transform.position.x, this.BotPos.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && NoInteraction == false)
        {
            if (base.transform.position.y == this.TopPos.y)
            {
                this.GoBot();
            }
            if (base.transform.position.y == this.BotPos.y)
            {
                this.GoTop();
            }
        }
    }

    public void GoBot()
    {
        this.rb2d.velocity = new Vector2(0f, -this.speed);
    }

    public void GoTop()
    {
        this.rb2d.velocity = new Vector2(0f, this.speed);
    }

    public Vector2 TopPos;

    public Vector2 BotPos;

    public float speed;

    public bool NoInteraction;

    public bool MoveOnStart;

    private Rigidbody2D rb2d;
}