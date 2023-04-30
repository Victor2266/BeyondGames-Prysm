using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dummy : MobGeneric
{
    public float totalDamage;

    public TMPro.TextMeshProUGUI totalDMGText;
    public TMPro.TextMeshProUGUI lastHittext;

    private void Start()
    {
        BSplat = GetComponent<BloodSplatterer>();

        lastHittext.text = "0";
        totalDMGText.text = "Total Damage: 0";
    }


    public override void TakeDamage(float amount)
    {
        totalDamage += amount;
        //Debug.Log(totalDamage);

        BSplat.Spray((int)amount / 3);
        UpdateHealthText(amount);
    }
    public void UpdateHealthText(float amount)
    {
        LeanTween.cancel(lastHittext.gameObject);
        LeanTween.cancel(totalDMGText.gameObject);

        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.scale(lastHittext.gameObject, Vector3.one * 0.2f + Vector3.one * amount / 70f, 0.1f).setEaseInOutBounce());
        sequence.append(LeanTween.scale(lastHittext.gameObject, Vector3.one * 0.2f, 0.3f).setEaseInOutCubic());

        lastHittext.color = Color.Lerp(Color.white, Color.red, amount / 100f);
        lastHittext.text = Mathf.Round(amount).ToString();

        totalDMGText.text = "Total Damage: " + Mathf.Round(totalDamage).ToString();
    }

    public void resetTotalDMG()
    {
        totalDamage = 0f;

        totalDMGText.text = "Total Damage: " + Mathf.Round(totalDamage).ToString();
    }
}
