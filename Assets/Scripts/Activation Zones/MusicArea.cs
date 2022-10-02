using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    private AudioSource audioSauzer;
    public GameObject BackgroundParent;

    public GameObject GlobalLight;
    private Light gLight;

    public float lightLevel;
    public Color lightColor;
    public float smoothTime;

    void Start()
    {
        audioSauzer = GetComponent<AudioSource>();
        GlobalLight = GameObject.FindGameObjectWithTag("GlobalLight");
        gLight = GlobalLight.GetComponent<Light>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSauzer = GetComponent<AudioSource>();
        if (collision.name == "Player")
        {

            audioSauzer.Play();

            if(BackgroundParent != null)
                BackgroundParent.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {

            audioSauzer.Stop();

            if (BackgroundParent != null)
                BackgroundParent.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {

            gLight.intensity = Mathf.SmoothStep(GlobalLight.GetComponent<Light>().intensity, lightLevel, smoothTime);

            gLight.color = Color.Lerp(gLight.color, lightColor, 0.1f);

        }
    }
        
}
