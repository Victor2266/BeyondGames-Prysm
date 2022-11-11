using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detachFromParent : MonoBehaviour
{
    public Transform oldParent;
    public ParticleSystem part;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldParent != null)
        {
            transform.position = oldParent.position;
        }
        else if (!dead)
        {
            dead = true;
            part.Emit(1);
            part.enableEmission = false;
        }
    }
}
