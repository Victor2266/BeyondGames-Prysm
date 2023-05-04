using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(MeshCollider))]
public class GunMagMinigame : MonoBehaviour
{
    public PlayerInput playerInput;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 velocity;
    private Vector2 joystickPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(DialogManager.instance.index == 8)
        {
            if (playerInput.actions != null)
                joystickPos = playerInput.actions["Navigate"].ReadValue<Vector2>();

            if (-9.175785f < transform.localPosition.x && transform.localPosition.x < 8.549582f && -25.27896f < transform.localPosition.y && transform.localPosition.y < -13.75938f)
                transform.position += (Vector3)joystickPos * 0.05f;
            else
            {
                LeanTween.cancel(gameObject);
                LeanTween.moveLocal(gameObject, new Vector3(-5.655519f, -22f), 1f).setEaseOutExpo();
            }

            if (3.137996f < transform.localPosition.x)
            {
                LeanTween.moveLocal(gameObject, new Vector3(4.288f, -22.68f, 0f), 0.65f).setEaseOutExpo().setOnComplete(IncrementIndex);
                IncrementIndex();
            }
        }
        else if (DialogManager.instance.index == 9)//wait until mag just outside of mag well position
        {
        }
        else if (DialogManager.instance.index == 10)
        {
            if (playerInput.actions != null)
                joystickPos = playerInput.actions["Navigate"].ReadValue<Vector2>();

            if (-25.27896f < transform.localPosition.y && transform.localPosition.y < -13.75938f)
                transform.localPosition = new Vector3(4.288f, transform.localPosition.y + joystickPos.y * 0.05f, 0f);
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, -25.27896f, 0f);
            }

            if(transform.localPosition.y > -19.49504f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, -19.49504f, 0f);

                DialogManager.instance.index++;
            }
        }
    }

    private void IncrementIndex()
    {
        DialogManager.instance.index++;
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        if (DialogManager.instance.index == 8)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, curPosition, ref velocity, 0.2f);
        }
        else if(DialogManager.instance.index == 10)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(transform.localPosition.x, curPosition.y, 0f), ref velocity, 0.2f);
        }
    }

}
