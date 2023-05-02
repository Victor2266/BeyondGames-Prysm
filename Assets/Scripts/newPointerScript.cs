using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPointerScript : MonoBehaviour
{
    public Transform lookAtThis;
    public float turn_speed;
    public float offset;

    private Vector3 dir;
    private float ZAngle;
    private Quaternion _lookRot;
    // Update is called once per frame
    void Update()
    {
        if(lookAtThis == null)
        {
            return;
        }
        dir = lookAtThis.position - transform.position;
        ZAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + offset;
        _lookRot = Quaternion.AngleAxis(ZAngle, Vector3.forward);

        if (turn_speed == 0)
        {
            transform.rotation = _lookRot;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRot, Time.deltaTime * turn_speed);
        }
    }

}
