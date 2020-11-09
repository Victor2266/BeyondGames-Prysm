using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineLength : MonoBehaviour
{
    SpriteRenderer Rend;
    DistanceJoint2D[] Goints;
    public Vector2 Orgin;
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        Rend = gameObject.GetComponent<SpriteRenderer>();
        Goints = gameObject.GetComponents<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rend.size.Set(Mathf.Sqrt(Mathf.Pow((boss.transform.position.x - Orgin.x),2) + Mathf.Pow((boss.transform.position.y - Orgin.y),2)),1f);
        Goints[0].distance = Mathf.Sqrt(Mathf.Pow((boss.transform.position.x - Orgin.x), 2) + Mathf.Pow((boss.transform.position.y - Orgin.y), 2)) / 2;
        Rend.size = new Vector2(Goints[0].distance * 1.75f, 1f);

        Goints[1].anchor = new Vector2(-Mathf.Sqrt(Mathf.Pow((boss.transform.position.x - Orgin.x), 2) + Mathf.Pow((boss.transform.position.y - Orgin.y), 2)) / 3f, 0f);
    }
}
