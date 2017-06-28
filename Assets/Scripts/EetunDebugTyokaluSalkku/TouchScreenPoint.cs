﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenPoint : MonoBehaviour
{
    //private CircleCollider2D _collider;
    private TouchController _touchController;
    public int x;
    public int y;

    void Start()
    {
        //_collider = GetComponent<CircleCollider2D>();
        _touchController = FindObjectOfType<TouchController>();
    }

    void Update()
    {
        // hei
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_touchController != null)
        {
            _touchController.OnTouchDetected(x, y);
            GetComponent<CircleCollider2D>().enabled = false;
           //  Debug.LogFormat("{0} {1}", x, y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
