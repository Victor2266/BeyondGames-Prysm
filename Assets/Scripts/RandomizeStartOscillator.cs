using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStartOscillator : MonoBehaviour
{
    public float startRange, endRange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<OscillateUpDown>().startPos.y = Random.Range(startRange, endRange);
    }
}
