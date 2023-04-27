using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dummy : MonoBehaviour
{
    public float totalDamage;
    public BloodSplatterer BSplat;
    public TMPro.TextMeshProUGUI text;

    private void Start()
    {
        BSplat = GetComponent<BloodSplatterer>();
    }
    public void FixedUpdate()
    {
        if(totalDamage > 0.01f)
        {
            totalDamage -= 0.01f;
        }
    }
    public void TakeDamage(float amount)
    {
        totalDamage += amount;
        Debug.Log(totalDamage);

        BSplat.Spray((int)amount / 3);
        UpdateHealthText(amount);
    }
    public void UpdateHealthText(float amount)
    {
        LeanTween.cancel(text.gameObject);
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.scale(text.gameObject, Vector3.one * 0.3f + Vector3.one * amount / 70f, 0.1f).setEaseInOutBounce());
        sequence.append(LeanTween.scale(text.gameObject, Vector3.one * 0.3f, 0.3f).setEaseInOutCubic());
        text.color = Color.Lerp(Color.white, Color.red, amount / 100f);
        text.text = Mathf.Round(amount).ToString();
    }

}
