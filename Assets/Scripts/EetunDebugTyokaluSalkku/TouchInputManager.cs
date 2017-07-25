using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchInputManager : MonoBehaviour
{
    public Rect[] hitboxes;
    public Transform[] Transforms;

    void Start()
    {
    }

    void OnDrawGizmos()
    {
        // Transforms = GetComponentsInChildren<Transform>();
        Gizmos.color = Color.red;



        for (int i = 0; i < hitboxes.Length; i++)
        {
            var rect = hitboxes[i];
            Gizmos.DrawWireCube(rect.center, rect.size);
        }
    }

    // kato onko hitboxsissa ja anna input
    // jos ulos niin... ???

    void Update()
    {
        for (int i = 0; i < hitboxes.Length; i++)
        {
            hitboxes[i].position = Transforms[i].position;
        }

        //      Touch[] touches = Input.touches;
        //      for (int i = 0; i < Input.touchCount; i++)
        //      {
        // 
        //      }
    }
}
