using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2OpeningScene : DialogTrigger
{
    public GameObject mainCamera; //start at -10, go to -2 pause 
    public GameObject FadeToBlack;
    public Button NextButton;
    public Button SkipButton;

    public float smoothTime = 2F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private bool notTriggeredYet;

    private float velo;
    private float velo2;
    private float velo3;
    private float audioVelo;

    public AudioSource audioSource;
    public AudioClip CutSceneSong;
    private bool playedSong = false;


    // Start is called before the first frame update
    void Start()
    {
        notTriggeredYet = true;
    }


    private IEnumerator DelaySentence(float WaitForAmount)
    {
        yield return new WaitForSeconds(WaitForAmount);
        DialogManager.instance.DisplayNextSentence();
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogManager.instance.index == 0)
        {
            TriggerDialog();
            targetPosition = new Vector3(0, -10, -10);
            //opening right eye ball
        }
        if (DialogManager.instance.index == 2)
        {
            //Zoomout
        }
        if (DialogManager.instance.index == 3)
        {
            //bring in hands from left and right
        }
        if (DialogManager.instance.index == 4)
        {
            //form white orb
        }
        if (DialogManager.instance.index == 5)
        {
            //fade in an image of the map within the orb
        }
        if (DialogManager.instance.index == 6)
        {
            //move around graveyard section but dont show hero

        }
        if (DialogManager.instance.index == 7)
        {
            //fade to black
            audioSource.volume = Mathf.SmoothDamp(audioSource.volume, 0f, ref audioVelo, 1f);  //change 0.01f to something else to adjust the rate of the volume dropping
        }

        if (DialogManager.instance.index == 8 && notTriggeredYet)//element 15
        {
            //knight hype excalibur pose and particle effects while panning and tilting downwards
            //hype music
            notTriggeredYet = true;


        }
        if (DialogManager.instance.index == 9 && notTriggeredYet)
        {
            //fade to black

        }
        else if (DialogManager.instance.index == 10)
        {
            //fade back to renka

        }
        if (DialogManager.instance.index == 11)
        {
            //renka smile

        }
        if (DialogManager.instance.index == 13)
        {
            //end
            FadeToBlack.SetActive(true);
            DialogManager.instance.EndDialog();
            StartCoroutine(MySceneManager.instance.SelectLevel("Level 2"));

        }
 
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
    }
}
