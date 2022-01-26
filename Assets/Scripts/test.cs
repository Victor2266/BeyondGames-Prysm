using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float ans = pfunction(1f, 4f);
        Debug.Log("This is the answer: " + ans);
    }


    float pfunction(float m, float k)
    {
        float answer;
        if (k == 0)
        {
            return 1;
        }
        return answer = m / (m + k) + (k / (m + k))*(1 - pfunction(m, k - 1));
    }
}
