using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSlideMinigame : MonoBehaviour
{

    public PlayerInput playerInput;
    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 velocity;

    private Vector2 joystickPos;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogManager.instance.index == 12)//MAG IS IN MAG WELL
        {
            if (playerInput.actions != null)
                joystickPos = playerInput.actions["Navigate"].ReadValue<Vector2>();

            if (2.21f <= transform.localPosition.x && transform.localPosition.x < 3.75)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + joystickPos.x * 0.05f, -17f, 0f);
            }
            else if (transform.localPosition.x < 2.21f)
            {
                transform.localPosition = new Vector3(2.21f, transform.localPosition.y, transform.localPosition.z);
            }
            else if(transform.localPosition.x >= 3.706559f)
            {
                LeanTween.cancel(gameObject);
                LeanTween.moveLocal(gameObject, new Vector3(2.21f, -17f, 0f), 1f).setEaseOutExpo().setOnComplete(ShootBullet);
                IncrementIndex();
            }
        }
    }

    public void ShootBullet()
    {
        Instantiate(bulletPrefab, new Vector3(-1.348f, -17.199f, 0f), transform.rotation);
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (DialogManager.instance.index == 12)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            if(2.21f <= transform.localPosition.x && transform.localPosition.x < 3.75)
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(curPosition.x, -17f, 0f), ref velocity, 0.1f);
            }
            else if (transform.localPosition.x < 2.21f)
            {
                //Update() sets the position back
            }
            else
            {
                //DialogManager.instance.index++; in update()
            }
        }
    }

    private void IncrementIndex()
    {
        DialogManager.instance.index++;
    }
}
