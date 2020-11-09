using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDoor : MonoBehaviour
{
    private void Start()
    {
        this.anim = base.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GreenCharged")
        {
            this.anim.SetTrigger("End");
            base.StartCoroutine(this.selfDestruct());
        }
    }

    private IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(5f);
        base.gameObject.SetActive(false);
        yield break;
    }

    public Animator anim;
}