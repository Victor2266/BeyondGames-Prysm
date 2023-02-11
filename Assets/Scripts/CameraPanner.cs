using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour
{

    public Vector3[] positions;
    public int index = 0;
    public Vector2 velocity;
    public float smoothTimeX;
    public float smoothTimeY;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.SmoothDamp(transform.position.x, positions[index].x, ref velocity.x, smoothTimeX);
        float num = Mathf.SmoothDamp(transform.position.y, positions[index].y, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(x, num, transform.position.z);

        if(Vector3.Distance(transform.position, positions[index]) < 1f)
        {
            index++;
        }
        if(index >= positions.Length)
        {
            index = 0;
        }
    }
}
