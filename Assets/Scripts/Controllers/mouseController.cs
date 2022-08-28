using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseController : MonoBehaviour
{
    Vector3 mouseWorldPosition;
    Vector3 Real_mouseWorldPosition;
    public Camera camera = null;
    public GameObject CameraPointerObj;
    public float distanceLimit;
    public bool isChildOfPlayer = true;

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

        //Debug.DrawRay(transform.position, mouseWorldPosition - transform.position, Color.green);
        Debug.DrawRay(transform.position, Real_mouseWorldPosition, Color.green);
    }
}
