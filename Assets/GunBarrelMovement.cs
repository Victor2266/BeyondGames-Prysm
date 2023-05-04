using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrelMovement : MonoBehaviour
{
    public GameObject gunSlide;
    public GameObject barrelPivot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zrot = (gunSlide.transform.position.x - 2.21f) / (3.75f - 2.21f) * -5f;
        barrelPivot.transform.localEulerAngles = new Vector3(0f, 0f, zrot);
    }
}
