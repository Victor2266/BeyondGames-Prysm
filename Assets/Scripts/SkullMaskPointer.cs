using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMaskPointer : MonoBehaviour
{
    private float xDiff;
    private float yDiff;

    private float zDiff;

    private float XAngle;
    private float YAngle;

    public GameObject subject;
    public GameObject MagnitudeSubject;
    public float DefaultZRotation = 0f;

    private Vector2 PointerPos;
    //public Camera CameraScript = new Camera();

    public float SphereRadius = 10f;
    private float magnitude;

    private void Update()
    {
        xDiff = subject.transform.localPosition.x - transform.localPosition.x;
        yDiff = subject.transform.localPosition.y - transform.localPosition.y;
        PointerPos = new Vector2(xDiff, yDiff).normalized;

        zDiff = Mathf.Sqrt(SphereRadius - Mathf.Pow(MagnitudeSubject.transform.localPosition.x, 2) - Mathf.Pow(MagnitudeSubject.transform.localPosition.y, 2));
        //Debug.Log(zDiff);

        this.XAngle = 57.29578f * Mathf.Atan(PointerPos.y / this.zDiff);
        this.YAngle = 57.29578f * Mathf.Atan(PointerPos.x / this.zDiff);



        //base.transform.eulerAngles = new Vector3(0.5f*XAngle, Mathf.Abs(0), 0);
        //base.transform.eulerAngles = new Vector3(0,-0.35f*YAngle, 0);
        magnitude = MagnitudeSubject.transform.localPosition.magnitude;
        base.transform.eulerAngles = new Vector3(XAngle * magnitude, -YAngle * magnitude, DefaultZRotation);
    }

}
