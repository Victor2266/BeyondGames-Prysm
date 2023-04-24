using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeLoadingBar : MonoBehaviour
{
    public float endinglength = 1280f;
    public float loadingTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scaleX(gameObject, endinglength, loadingTime).setEase(LeanTweenType.easeInOutExpo);
    }
}
