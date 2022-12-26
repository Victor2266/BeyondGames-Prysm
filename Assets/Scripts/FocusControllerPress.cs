using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FocusControllerPress : MonoBehaviour
{
    public GameObject player;
    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //check for controller press
        if (playerInput.actions["Navigate"].WasPressedThisFrame() && (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.active))
        {
            EventSystem.current.SetSelectedGameObject(null);
            GameObject defaultMenuOption = GameObject.FindGameObjectWithTag("DefaultOption");
            if (defaultMenuOption != null)
                EventSystem.current.SetSelectedGameObject(defaultMenuOption);
        }
        else if (playerInput.actions["Point"].WasPerformedThisFrame())
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
