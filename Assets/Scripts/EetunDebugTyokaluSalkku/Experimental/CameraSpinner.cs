using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraSpinner : MonoBehaviour
{
    public float RotationRate;
    public float MaxZoom;
    public float ZoomSpeed;

    private Transform _parent;

    private float _defaultZoom;
    private float _zoomFactor;

    private bool _spinning;
    private float _t = 0f;

    private float _increment;

    void Start()
    {
        _defaultZoom = Camera.main.orthographicSize;
        _increment = ZoomSpeed / 10f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (!_spinning)
                StartSpinning();
            else
                StopSpinning();
        }

        if (_spinning)
        {
            if (_t >= 1f)
            {
                if (transform.rotation.eulerAngles.z >= 90f)
                {
                    transform.Rotate(Vector3.back, RotationRate);
                }
            }
            else
            {
                transform.Rotate(Vector3.back, RotationRate);
            }
            _t += _increment;
        }
        else
        {
            _t -= _increment;
        }

        Camera.main.orthographicSize = Mathf.Lerp(_defaultZoom, MaxZoom, _t);
        _t = Mathf.Clamp(_t, 0f, 1f);
    }

    public void StartSpinning()
    {
        _parent = transform.parent;
        transform.parent = null;
        _spinning = true;
    }

    public void StopSpinning()
    {
        transform.parent = _parent;
        _spinning = false;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    //IEnumerator SpinBack()
    //{
    //    Quaternion TargetRotation = Quaternion.Euler(0f, 0f, 0f);
    //    Quaternion from = transform.rotation;

    //    while (transform.rotation != TargetRotation)
    //    {
    //        transform.rotation = Quaternion.Lerp(from, TargetRotation, Time.time * RotationRate / 100f);
    //        yield return null;
    //    }
    //}
}
