using UnityEngine;
using System.Collections;

public class DestroyOnTime : MonoBehaviour
{

    public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}