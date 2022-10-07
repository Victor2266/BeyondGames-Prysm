using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{

    #region Singleton
    public static BloodManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Blood found!");
            Destroy(this);
            return;
        }
        instance = this;

    }

    #endregion


    public Queue<GameObject> stains = new Queue<GameObject>();

    public int MaxStains;
    public int currentStains = 0;
}
