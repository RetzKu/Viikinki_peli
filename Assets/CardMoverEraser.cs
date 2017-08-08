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
    private bool _newCardFlag = false;
    CardMoverEraser[] kortit;

    void Start()
    {
        //_startPosition = transform.position;
        kortit = FindObjectsOfType(typeof(CardMoverEraser)) as CardMoverEraser[];
        foreach (CardMoverEraser kortti in kortit)
        {
            kortti.WildCardAppears();
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Selected
        print("click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // hover stuff
    }

    // korttien transformit on lokattu =( // fixered
    public void OnPointerUp(PointerEventData eventData)
    {
        print(Vector2.Distance((Vector2)transform.position, _startPosition) );
        float dis = Vector2.Distance(transform.position, _startPosition);

        if (Vector2.Distance((Vector2)transform.position, _startPosition) >= DropDistance)
        {
            GetComponentInParent<DeckScript>().dragFalse();
            // poista, jostain inventorysta
            //Destroy(gameObject);

            string tempString = gameObject.name.Substring(gameObject.name.Length - 1, 1);
            int tempInt = int.Parse(tempString);
            GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.DropItem(tempInt);

        }
        else 
        {
            // kotiin
            _endPosition = eventData.position;
            _returnBack = true;
            GetComponentInParent<DeckScript>().dragFalse();
        }
    }



    void Update()
    {
        if (_newCardFlag)
        {
            _startPosition = transform.localPosition;
            _newCardFlag = false;
        }

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
        GetComponentInParent<DeckScript>().dragTrue();
        transform.position = eventData.position;
        _returnBack = false;
        _t = 1.0f;
    }
    
    private void WildCardAppears()
    {
        _newCardFlag = true;
    }

    private void LoadOpenPosition()
    {
        Vector3 tempVec;
        string tempString = gameObject.name.Substring(gameObject.name.Length - 1, 1);
        int tempInt = int.Parse(tempString);
        tempVec = GetComponentInParent<DeckScript>().OpenInvPositions(tempInt);
       // _startPosition = tempVec;
    }

    public void ApplyOpenPosition()
    {
        kortit = FindObjectsOfType(typeof(CardMoverEraser)) as CardMoverEraser[];
        foreach (CardMoverEraser kortti in kortit)
        {
            kortti.LoadOpenPosition();
        };
    }
}
