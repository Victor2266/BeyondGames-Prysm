using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3OpeningScene : DialogTrigger
{
    public GameObject mainCamera; //start at -10, go to -2 pause 
    public GameObject FadeToBlack;
    public Button NextButton;
    public Button SkipButton;

    public AudioSource audioSource;
    public AudioClip CutSceneSong;

    public AudioSource[] Sounds;

    private bool notTriggeredYet;

    public GameObject gunMag;

    // Start is called before the first frame update
    void Start()
    {
        notTriggeredYet = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogManager.instance.index == 0)
        {
            TriggerDialog();

        }
        if (DialogManager.instance.index == 2)//hahaha, element 1
        {
            if(!Sounds[0].isPlaying)
                Sounds[0].Play();
        }
        if (DialogManager.instance.index == 3)//tsktsk, ele 2
        {
            if (!Sounds[1].isPlaying)
                Sounds[1].Play();
        }
        if (DialogManager.instance.index == 4)//dadun, ele 3
        {
            if (!Sounds[2].isPlaying)
                Sounds[2].Play();
            DialogManager.instance.index++;
            LeanTween.moveY(mainCamera, -10f, 1f).setEaseOutExpo();
        }
        if (DialogManager.instance.index == 6)//dadun,ele 4
        {
            LeanTween.moveY(mainCamera, -19.5f, 1f).setEaseOutExpo();
            if (DialogManager.instance.animator.GetBool("IsOpen") == false)
            {
                DialogManager.instance.index = 7;
            }
        }
        if (DialogManager.instance.index == 7)//gun minigame
        {
            LeanTween.moveLocalY(gunMag, -22f, 1f).setEaseOutExpo();
            DialogManager.instance.index++;
        }
        if (DialogManager.instance.index == 11)//click sound
        {
            Sounds[3].Play();
            DialogManager.instance.index++;
        }

    }
}
