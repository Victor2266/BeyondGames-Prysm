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
    public SpriteRenderer indicatorImage;
    public Light indicatorLight;

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
        else if (DialogManager.instance.index == 2)//hahaha, element 1
        {
            if(!Sounds[0].isPlaying)
                Sounds[0].Play();
        }
        else if (DialogManager.instance.index == 3)//tsktsk, ele 2
        {
            if (!Sounds[1].isPlaying)
                Sounds[1].Play();
        }
        else if (DialogManager.instance.index == 4)//dadun, ele 3
        {
            if (!Sounds[2].isPlaying)
                Sounds[2].Play();
            DialogManager.instance.index++;
            LeanTween.moveY(mainCamera, -10f, 1f).setEaseOutExpo();
        }
        else if (DialogManager.instance.index == 6)//dadun,ele 4
        {
            LeanTween.moveY(mainCamera, -19.5f, 1f).setEaseOutExpo();
            if (DialogManager.instance.animator.GetBool("IsOpen") == false)
            {
                DialogManager.instance.index = 7;
            }
        }
        else if (DialogManager.instance.index == 7)//gun minigame
        {
            LeanTween.moveLocalY(gunMag, -22f, 1f).setEaseOutExpo();
            DialogManager.instance.index++;
        }
        else if (DialogManager.instance.index == 11)//click sound
        {
            Sounds[3].Play();
            LeanTween.value(0f, 1f, 0.2f).setOnUpdate((float val) => { indicatorImage.color = Color.Lerp(Color.white, new Color(1f, 0.6104861f, 0), val); } ).setEaseOutExpo();
            indicatorLight.gameObject.SetActive(true);
            DialogManager.instance.index++;
        }
        else if (DialogManager.instance.index == 13)//click sound
        {
            Sounds[4].Play();
            LeanTween.value(0f, 1f, 0.2f).setOnUpdate((float val) => { indicatorImage.color = Color.Lerp(Color.white, Color.green, val); }).setEaseOutExpo();
            DialogManager.instance.index++;
        }
        else if (DialogManager.instance.index == 15)
        {
            //end
            FadeToBlack.SetActive(true);
            DialogManager.instance.EndDialog();
            StartCoroutine(MySceneManager.instance.SelectLevel("Level 3"));
            DialogManager.instance.index++;

        }
    }

}
