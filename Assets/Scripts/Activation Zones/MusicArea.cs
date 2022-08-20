using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    private AudioSource audioSauzer;
    public float lightLevel;
    public GameObject BackgroundParent;

    public GameObject GlobalLight;

    public float smoothTime;

    void Start()
    {
        audioSauzer = GetComponent<AudioSource>();
        GlobalLight = GameObject.FindGameObjectWithTag("GlobalLight");
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSauzer = GetComponent<AudioSource>();
        if (collision.name == "Player")
        {

            audioSauzer.Play();

            BackgroundParent.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {

            audioSauzer.Stop();

            BackgroundParent.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {

            GlobalLight.GetComponent<Light>().intensity = Mathf.SmoothStep(GlobalLight.GetComponent<Light>().intensity, lightLevel, smoothTime);

        }
    }
        
}
