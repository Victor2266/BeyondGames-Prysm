using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScene : DialogTrigger
{
    public GameObject fallingParticles;
    public ParticleSystem FlyingUpWhiteLines;//Start Lifetime go from 1 to 5, start size go from 0.02 to 0.05
    public GameObject SoulAtom; //rise to meet camera 
    public GameObject mainCamera; //start at -10, go to -2 pause 
    public GameObject PCScreen;
    public GameObject FadeToBlack;
    public GameObject DESKTOP;
    public Button NextButton;
    public Button SkipButton;
    public ParticleSystem SoulAtomDeath;
    public GameObject SkullMask;

    public float smoothTime = 2F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private bool notTriggeredYet;

    private float velo;
    private float velo2;
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
        }
        if (DialogManager.instance.index == 3)
        {
            targetPosition = new Vector3(0, -2, -10);
        }
        if (DialogManager.instance.index == 7)
        {
            targetPosition = new Vector3(0, 0, -10);
        }
        if (DialogManager.instance.index == 10)
        {
            PCScreen.SetActive(true);
        }
        if (DialogManager.instance.index == 14)
        {
            targetPosition = new Vector3(0, -2, -10);
        }
        if (DialogManager.instance.index == 15 && notTriggeredYet)
        {
            FadeToBlack.SetActive(true);
            NextButton.interactable = false;
            SkipButton.interactable = false;
            StartCoroutine(DelaySentence(5f));
            notTriggeredYet = false;
        }
        if (DialogManager.instance.index == 16)//element 15
        {
            notTriggeredYet = true;
            NextButton.interactable = true;
            SkipButton.interactable = true;
            targetPosition = new Vector3(0, 0, -10);
            FadeToBlack.SetActive(false);
            DESKTOP.SetActive(false);
            fallingParticles.SetActive(true);
        }
        if (DialogManager.instance.index == 17 && notTriggeredYet)
        {
            notTriggeredYet = false;
            smoothTime = 5f;
            targetPosition = new Vector3(0, -30.5f, -10);
            SoulAtom.SetActive(true);

            NextButton.interactable = false;
            SkipButton.interactable = false;

            StartCoroutine(DelaySentence(15f));
            //shrink flying particles

        }
        else if (DialogManager.instance.index == 17)
        {
            FlyingUpWhiteLines.startLifetime = Mathf.SmoothDamp(FlyingUpWhiteLines.startLifetime, 0f, ref velo, 5f);//Start Lifetime go from 1 to 5, start size go from 0.02 to 0.05
            FlyingUpWhiteLines.startSize = Mathf.SmoothDamp(FlyingUpWhiteLines.startSize, 0f, ref velo2, 5f);//Start Lifetime go from 1 to 5, start size go from 0.02 to 0.05
        }
        if (DialogManager.instance.index == 18)
        {
            notTriggeredYet = true;
            fallingParticles.SetActive(false);
            DialogManager.instance.nameText.text = "<color=#5888FF>Renka</color>";
            NextButton.interactable = true;
            SkipButton.interactable = true;
            velo = 0f;
        }
        if (DialogManager.instance.index == 24)
        {
            SoulAtomDeath.startLifetime = Mathf.SmoothDamp(SoulAtomDeath.startLifetime, 0f, ref velo, 1.5f);//Start Lifetime go from 1 to 5, start size go from 0.02 to 0.05
            SkullMask.SetActive(true);
        }
        if (DialogManager.instance.index > 24)
        {
            SoulAtomDeath.startLifetime = Mathf.SmoothDamp(SoulAtomDeath.startLifetime, 0f, ref velo, 1.5f);//Start Lifetime go from 1 to 5, start size go from 0.02 to 0.05
        }
        if (DialogManager.instance.index == 28)
        {
            FadeToBlack.SetActive(true);
            DialogManager.instance.EndDialog();
            StartCoroutine(MySceneManager.instance.SelectLevel("Level 1"));
        }
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
    }
}
