using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class popDeletion : MonoBehaviour
{
    [SerializeField]
    float ShakeMagnitude = 0f;
    [SerializeField]
    float ShakeRoughness = 0f;
    [SerializeField]
    float ShakeFadeIn = 0f;
    [SerializeField]
    float ShakeFadeOut = 0f;

    Animator anim;

    // Use this for initialization
    private void Start()
    {
        CameraShaker.Instance.ShakeOnce(ShakeMagnitude, ShakeRoughness, ShakeFadeIn, ShakeFadeOut);
        anim = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("popped") && anim != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(WaitThenDestroy());
        }
        //Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }
}
