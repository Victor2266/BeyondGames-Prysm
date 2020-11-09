using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRoomTemplate : MonoBehaviour
{
    public Camera RoomCam;
    public GameObject ITEM1;
    public int cost;
    public GameObject NoMoneyError;

    public Camera MainCam;
    public GameObject Player;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (Input.GetButtonDown("Use/Interact"))
            {
                if(PlayerController.Lives >= cost){
                    PlayerController.Lives -= cost;
                    GameObject gameObject = Instantiate(ITEM1, transform.position, transform.rotation);
                }
                else
                {
                    NoMoneyError.SetActive(true);
                }
                StartCoroutine(PauseDelay());
            }
        }
    }
    IEnumerator PauseDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            RoomCam.gameObject.SetActive(true);
            Player.GetComponents<mouseController>()[1].camera = RoomCam;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        NoMoneyError.SetActive(false);
        if (collision.name == "Player")
        {
            RoomCam.gameObject.SetActive(false);
            Player.GetComponents<mouseController>()[1].camera = MainCam;
        }
    }
}
