using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLightController : MonoBehaviour
{

    public float layer1;
    public float layer2;
    public float layer3;
    public float layer4;

    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (player.transform.localPosition.y < layer1)
        {
            GetComponent<Light>().intensity = 0f;
        }
    }
}
