using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InGameDialogStarter : Interactable
{
    public GameObject tooltipPrefab;
    private GameObject tooltip;
    private TextMeshPro toolTipText;
    private SpriteRenderer toolTipArrow;

    public string name;

    private void Start()
    {
        tooltip = Instantiate(tooltipPrefab);

        tooltip.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, -3f);
        tooltip.transform.rotation = Quaternion.identity;
        tooltip.transform.localScale = new Vector3(1f, 1f, 1f);

        //tooltip.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0.5f, 0f);
        //tooltip.GetComponent<RectTransform>().eulerAngles = new Vector3(0f, 0f, 0f);
        //tooltip.transform.sc = new Vector2(1f, 1f);

        toolTipText = tooltip.GetComponent<TextMeshPro>();
        toolTipArrow = tooltip.GetComponentInChildren<SpriteRenderer>();

        DialogManager.instance.onEndDialog += ResetDialog;
    }

    // When the player interacts with the item
    public override void Interact()
    {
        base.Interact();

        StartDialog();   // Pick it up!
    }

    // Pick up the item
    void StartDialog()
    {
        Debug.Log("Talking To: " + name);
        GetComponent<DialogTrigger>().TriggerDialog();
    }

    void Update()
    {
        tooltip.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, -3f);

        // If we are close enough
        float distance = Vector2.Distance(player.position, interactionTransform.position);
        if (distance <= radius)
        {
            // display the tooltip with the item name
            toolTipText.text = name;
            toolTipArrow.color = Color.white;
        }
        else
        {
            toolTipText.text = "";
            toolTipArrow.color = new Color(1f, 1f, 1f, 0.5f);
            if (hasInteracted)
            {
                DialogManager.instance.EndDialog();
            }
        }
        base.Update();
    }

    public void ResetDialog()//the close button also usees this function
    {
        hasInteracted = false;
    }
}
