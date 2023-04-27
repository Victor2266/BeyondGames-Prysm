using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterer : MonoBehaviour
{
    public GameObject bloodSpray;
    private Vector3 collisionPosition;
    public void SetCollision(Vector2 pos)//setting up position and direction of blood splatter
    {
        collisionPosition = pos;
        bloodDir = transform.position - new Vector3(pos.x, pos.y, 1f);
        ZAngle = Mathf.Atan2(bloodDir.y, bloodDir.x) * Mathf.Rad2Deg + bloodOffset;
        _lookRot = Quaternion.AngleAxis(ZAngle, Vector3.forward);
        bloodRot = _lookRot;
    }
    private Vector3 bloodDir;
    private Quaternion bloodRot;
    public float bloodOffset;
    private float ZAngle;
    private Quaternion _lookRot;

    private GameObject lastSpray;
    public void Spray(float amount)
    {
        lastSpray = Instantiate(bloodSpray, collisionPosition, bloodRot);
        lastSpray.GetComponent<ParticleSystem>().Emit((int)amount);//amount of splatter particles
    }


}
