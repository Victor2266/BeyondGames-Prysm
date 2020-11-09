using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    private AudioSource audioSauzer;

    void Start()
    {
        audioSauzer = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSauzer = GetComponent<AudioSource>();
        if (collision.name == "Player")
        {

            audioSauzer.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {

            audioSauzer.Stop();
        }
    }
}
