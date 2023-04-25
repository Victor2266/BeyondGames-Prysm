using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class MoveHandsOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// required interface when using the OnSelect method.
{
    private GameObject mainMenu;
    private MainMenuScript mainMenuScript;

    void Start()
    {
        mainMenu = GameObject.FindGameObjectWithTag("Canvas");
        mainMenuScript = mainMenu.GetComponent<MainMenuScript>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mainMenuScript.MoveHandsClose();
        mainMenuScript.MoveHands(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //mainMenuScript.MoveHandsAway();
    }
}