using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mouseController : MonoBehaviour
{
    Vector3 mouseWorldPosition;
    Vector3 Real_mouseWorldPosition;
    public Camera camera = null;
    public GameObject CameraPointerObj;
    public float distanceLimit;
    public bool isChildOfPlayer = true;

    public PlayerInput playerInput;
    public WeaponController weaponController;

    public static bool useStickPos = false;
    private Vector2 lastJoyPos;
    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            if (!isChildOfPlayer)
            {
                CameraPointerObj = GameObject.FindGameObjectWithTag("Cursor");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        Vector2 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);

        if (isChildOfPlayer)
        {
            if (distanceLimit > 0){
                Real_mouseWorldPosition = Vector2.ClampMagnitude(mouseWorldPosition - new Vector2(transform.position.x, transform.position.y), distanceLimit);
            }
            else{
                Real_mouseWorldPosition = mouseWorldPosition - new Vector2(transform.position.x, transform.position.y);
            }
        }
        else
        {
            Real_mouseWorldPosition = mouseWorldPosition;
        }
        /*
        Real_mouseWorldPosition = mouseWorldPosition - new Vector2(transform.position.x, transform.position.y);
        
        if (Real_mouseWorldPosition.magnitude > 1f)
        {
            Real_mouseWorldPosition = Real_mouseWorldPosition.normalized * 1;
        }
        */
        CameraPointerObj.transform.localPosition = new Vector3(Real_mouseWorldPosition.x , Real_mouseWorldPosition.y, 0);

        Vector2 joystickPos = playerInput.actions["Primary Attack [Gamepad]"].ReadValue<Vector2>();
        if (joystickPos != Vector2.zero)
        {
            if (weaponController != null)
            {
                CameraPointerObj.transform.localPosition = joystickPos * weaponController.ReachLength;
            }
            else
            {
                CameraPointerObj.transform.localPosition = joystickPos;
            }
            useStickPos = true;
            lastJoyPos = joystickPos;
        }
        else if (useStickPos)
        {
            CameraPointerObj.transform.localPosition = Vector2.ClampMagnitude(lastJoyPos.normalized, 0.1f);
            useStickPos = !playerInput.actions["Mouse"].WasPerformedThisFrame();
        }


        //Debug.DrawRay(transform.position, mouseWorldPosition - transform.position, Color.green);
        Debug.DrawRay(transform.position, Real_mouseWorldPosition, Color.green);
    }

}
