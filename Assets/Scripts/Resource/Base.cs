using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseManager : MonoBehaviour
{
    public BaseManager Instance = null;
    void Awake()
    {
        Instance = this;
    }

    // FastTravel menut jne tänne
    // lisää unlockatut campit tänne
}
