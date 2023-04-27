using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dummy : MonoBehaviour
{
    public float totalDamage;
    public BloodSplatterer BSplat;

    public TMPro.TextMeshProUGUI totalDMGtext;
    public TMPro.TextMeshProUGUI lastHittext;

    private void Start()
    {
        BSplat = GetComponent<BloodSplatterer>();
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
        LeanTween.cancel(lastHittext.gameObject);
        LeanTween.cancel(totalDMGtext.gameObject);

        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.scale(lastHittext.gameObject, Vector3.one * 0.3f + Vector3.one * amount / 70f, 0.1f).setEaseInOutBounce());
        sequence.append(LeanTween.scale(lastHittext.gameObject, Vector3.one * 0.3f, 0.3f).setEaseInOutCubic());

        lastHittext.color = Color.Lerp(Color.white, Color.red, amount / 100f);
        lastHittext.text = Mathf.Round(amount).ToString();

        totalDMGtext.text = "Total Damage: " + Mathf.Round(totalDamage).ToString();
    }

}
