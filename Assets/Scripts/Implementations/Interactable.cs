using UnityEngine;
using UnityEngine.InputSystem;

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
	protected PlayerInput playerInput;


	protected bool hasInteracted = false; // Have we already interacted with the object?

    private void OnEnable()
    {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerInput = player.gameObject.GetComponent<PlayerInput>();
	}
	public virtual void Interact()
	{
		// This method is meant to be overwritten
		Debug.Log("Interacting with " + transform.name);
	}

	public void Update()
	{
		// If we are currently being focused
		// and we haven't already interacted with the object
		if (!hasInteracted && !PauseMenuScript.isPaused)
		{
			// If we are close enough
			float distance = Vector2.Distance(player.position, interactionTransform.position);
			if (distance <= radius && playerInput.actions["Interact"].ReadValue<float>() > 0f)
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