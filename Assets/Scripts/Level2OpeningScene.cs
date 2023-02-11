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
    public GameObject hands, RHand, LHand;
    public GameObject spikyEdges, soulOrb, soulLight, graveVideo, fullGlow, ArcLight, CenterSpiral, eyemask, smile;

    public Animator eye;

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
            DialogManager.instance.nameText.text = "<color=#5888FF>Renka</color>";
            targetPosition = new Vector3(0, -10, -10);
            //opening right eye ball

        }
        if (DialogManager.instance.index == 2)//Huh,
        {
            //Zoomout
            if (notTriggeredYet)
            {
                LeanTween.value(mainCamera, setCameraSize, 5f, 7f, 4f).setEase(LeanTweenType.easeOutElastic);
                notTriggeredYet = false;
            }
        }
        if (DialogManager.instance.index == 3)//Maybe you have some potential after all
        {
            //bring in hands from left and right
            if (!notTriggeredYet)
            {
                LeanTween.move(hands, new Vector3(0, 2.5f, 0), 2f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.value(RHand, setRHandPos, RHand.transform.position.x, 4.28752f, 3f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.value(LHand, setLHandPos, LHand.transform.position.x, -5.96248f, 3f).setEase(LeanTweenType.easeOutQuad);
                targetPosition = new Vector3(-1.05f, -15f, -10);
                notTriggeredYet = true;
            }
        }
        if (DialogManager.instance.index == 4)//Then I have another mission for you
        {
            //form white orb
            //Spiky edges eyes activate
            if (notTriggeredYet)
            {
                notTriggeredYet = false;
                spikyEdges.SetActive(true);
                eyemask.SetActive(false);
                LeanTween.move(soulOrb, new Vector3(-0.9643354f, -17.96564f, 2.823045f), 2f).setEase(LeanTweenType.easeOutQuad);
                soulOrb.transform.SetParent(hands.transform, true);
                LeanTween.value(soulOrb, setSoulSize, 1.5f, 11.5f, 6f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.value(soulOrb, setSoulcolor, 98f, 64f, 6f).setEase(LeanTweenType.easeOutQuad);
            }

        }
        if (DialogManager.instance.index == 5)//before you hunt any big targets you're gonna need some better weapons.
        {
            //fade in an image of the map within the orb
            if (!notTriggeredYet)
            {
                notTriggeredYet = true;

                targetPosition = new Vector3(-1.046757f, -16.69f, -10f);
                LeanTween.value(mainCamera, setCameraSize, 7f, 0.3f, 4f).setEase(LeanTweenType.easeOutQuad);

                graveVideo.SetActive(true);

            }
        }
        if (DialogManager.instance.index == 6)//There's a graveyard in the forest outskirts that holds the graves of many former heros.
        {
            //move around graveyard section but dont show hero

        }
        if (DialogManager.instance.index == 7)//You're gonna pillage those graves for the weapons buried within them.
        {
            //fade to black
            if (notTriggeredYet)
            {
                notTriggeredYet = false;

                LeanTween.value(mainCamera, setVideoAlpha, 1f, 0f, 4f).setEase(LeanTweenType.easeOutQuad);

            }
            audioSource.volume = Mathf.SmoothDamp(audioSource.volume, 0f, ref audioVelo, 1f);  //change 0.01f to something else to adjust the rate of the volume dropping

        }

        if (DialogManager.instance.index == 8)//element 15 The only problem is that the hero of Giha has been at the graveyard for the past 5 days.
        {
            //cut music
            if (!notTriggeredYet)
            {
                notTriggeredYet = true;
                audioSource.Stop();
                audioSource.volume = 1f;

                graveVideo.SetActive(false);
            }


        }
        if (DialogManager.instance.index == 9 && notTriggeredYet)
        {
            //knight hype excalibur pose and particle effects while panning and tilting downwards
            //hype music
            if (notTriggeredYet)
            {

                LeanTween.value(mainCamera, setCameraSize, 0.3f, 1.2f, 4f).setEase(LeanTweenType.easeOutQuad);
                targetPosition = new Vector3(0.4f, -32.97f, -10f);
                notTriggeredYet = false;
                audioSource.clip = CutSceneSong;
                audioSource.Play();

            }

        }
        else if (DialogManager.instance.index == 10)//I'm not sure why he's there.
        {
            //fade back to renka
            if (!notTriggeredYet)
            {
                LeanTween.value(mainCamera, setCameraSize, 1.2f, 5f, 4f).setEase(LeanTweenType.easeOutQuad);
                targetPosition = new Vector3(-0.23f, -10, -10f);
                notTriggeredYet = true;

            }

        }
        else if (DialogManager.instance.index == 11)//But there's no way he is going to let you pillage the graves in front of him.
        {
            //renka smile

            if (notTriggeredYet)
            {
                LeanTween.move(hands, new Vector3(0, -1.4f, 0), 2f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.scale(smile, new Vector3(1f, 0.75f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
                fullGlow.SetActive(true);

                notTriggeredYet = false;

            }

        }
        else if (DialogManager.instance.index == 12)//So, defeat him and consume his soul as well.
        {
            //activate glowing spikes full
            if (!notTriggeredYet)
            {
                notTriggeredYet = true;


                ArcLight.SetActive(true);
                LeanTween.value(mainCamera, setArclightArc, 1f, 360f, 2f).setEase(LeanTweenType.easeOutQuad);
            }

        }
        else if (DialogManager.instance.index == 13)//Two birds with one stone! Good Luck!
        {
            //end
            if (notTriggeredYet)
            {
                notTriggeredYet = false;

                CenterSpiral.SetActive(true);


                LeanTween.value(mainCamera, setArclightSpeed, 0.2f, 2f, 4f).setEase(LeanTweenType.easeOutQuad);

                StartCoroutine(waitThenIndex(4f));
            }

        }
        else if (DialogManager.instance.index == 14)
        {
            //end
            if (!notTriggeredYet)
            {
                notTriggeredYet = true;

                FadeToBlack.SetActive(true);
                DialogManager.instance.EndDialog();
                StartCoroutine(MySceneManager.instance.SelectLevel("Level 2"));
            }


        }

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void setCameraSize(float size)
    {
        mainCamera.GetComponent<Camera>().orthographicSize = size;
    }
    public void setRHandPos(float pos)
    {
        RHand.transform.position = new Vector3(pos, RHand.transform.position.y, RHand.transform.position.z);
    }
    public void setLHandPos(float pos)
    {
        LHand.transform.position = new Vector3(pos, LHand.transform.position.y, LHand.transform.position.z);
    }
    public void setSoulSize(float s)
    {
        soulOrb.GetComponent<ParticleSystem>().startSize = s;

        ParticleSystem.VelocityOverLifetimeModule vm = soulOrb.GetComponent<ParticleSystem>().velocityOverLifetime;

        vm.enabled = false;


        soulLight.SetActive(true);
        soulLight.GetComponent<ArtificialLightFlicker>().limit = s / 2.55f;

    }
    
    public void setSoulcolor(float s)
    {
        soulOrb.GetComponent<ParticleSystem>().startColor = new Color(61f/255f, 133f/255f, 245f/255f, s/255f);
    }
    public void setVideoAlpha(float s)
    {
        graveVideo.GetComponent<UnityEngine.Video.VideoPlayer>().targetCameraAlpha = s;
    }
    public void setArclightArc(float s)
    {
        ParticleSystem.ShapeModule sm = ArcLight.GetComponent<ParticleSystem>().shape;

        sm.arc = s;

    }
    public void setArclightSpeed(float s)
    {
        ArcLight.GetComponent<ParticleSystem>().startSpeed = s;
    }

    private IEnumerator waitThenIndex(float f)
    {
        yield return new WaitForSeconds(f);
        DialogManager.instance.index++;
    }
}
