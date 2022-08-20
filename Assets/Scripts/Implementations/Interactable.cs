using UnityEngine;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

public class Interactable : MonoBehaviour
{

	public float radius = 1.5f;               // How close do we need to be to interact?
	public Transform interactionTransform;  // The transform from where we interact in case you want to offset it

	public Transform player;       // Reference to the player transform

	bool hasInteracted = false; // Have we already interacted with the object?

    private void OnEnable()
    {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	public virtual void Interact()
	{
		// This method is meant to be overwritten
		Debug.Log("Interacting with " + transform.name);
	}

	void Update()
	{
		// If we are currently being focused
		// and we haven't already interacted with the object
		if (!hasInteracted)
		{
			// If we are close enough
			float distance = Vector2.Distance(player.position, interactionTransform.position);
			if (distance <= radius && Input.GetButtonDown("Use/Interact"))
			{
				// Interact with the object
				Interact();
				hasInteracted = true;
			}
		}
	}


	// Draw our radius in the editor
	void OnDrawGizmosSelected()
	{
		if (interactionTransform == null)
			interactionTransform = transform;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}