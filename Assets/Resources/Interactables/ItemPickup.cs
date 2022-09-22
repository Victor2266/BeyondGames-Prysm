using UnityEngine;
using TMPro;

public class ItemPickup : Interactable
{

	public Item item;   // Item to put in the inventory on pickup
    public GameObject tooltipPrefab;
    private GameObject tooltip;
    private TextMeshPro toolTipText;
    private SpriteRenderer toolTipArrow;

    private string name;

    private void Start()
    {
        tooltip = Instantiate(tooltipPrefab, transform);
        tooltip.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0.5f, 0f);

        toolTipText = tooltip.GetComponent<TextMeshPro>();
        toolTipArrow = tooltip.GetComponentInChildren<SpriteRenderer>();

        name = item.name;
    }

    // When the player interacts with the item
    public override void Interact()
	{
		base.Interact();

		PickUp();   // Pick it up!
	}

	// Pick up the item
	void PickUp()
	{
		Debug.Log("Picking up " + item.name);
		bool wasPickedUp = Inventory.instance.Add(item);    // Add to inventory

		// If successfully picked up
		if (wasPickedUp)
			Destroy(gameObject);    // Destroy item from scene
    }

    void Update()
    {
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
        }
    }
}