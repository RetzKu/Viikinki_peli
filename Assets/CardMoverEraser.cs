using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMoverEraser : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, 
    IPointerUpHandler, IPointerDownHandler, 
    IDragHandler
{
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    public float DropDistance = 350f;
    public float ReturnSpeed = 1f;
    private float _t = 1.0f;
    private bool _returnBack;

    void Start()
    {
        _startPosition = transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("click");
        // selected
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // hover stuff
    }

    // korttien transformit on lokattu =(
    public void OnPointerUp(PointerEventData eventData)
    {
        print(Vector2.Distance((Vector2)transform.position, _startPosition) );
        float dis = Vector2.Distance(transform.position, _startPosition);

        if (Vector2.Distance((Vector2)transform.position, _startPosition) >= DropDistance)
        {
            // poista, jostain inventorysta
            Destroy(gameObject);
        }
        else 
        {
            // kotiin
            _endPosition = eventData.position;
            _returnBack = true;
        }
    }

    void Update()
    {
        if (_returnBack)
        {
            transform.position = Vector2.Lerp(_startPosition, _endPosition, _t);
            _t -= Time.deltaTime * ReturnSpeed;

            if (_t < 0.0f)
            {
                _returnBack = false;
                _t = 1f;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        // tohon bool

        transform.position = eventData.position;
        _returnBack = false;
        _t = 1.0f;
    }
}
