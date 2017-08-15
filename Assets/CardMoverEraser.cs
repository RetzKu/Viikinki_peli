using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMoverEraser : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler,
    IPointerUpHandler, IPointerDownHandler,
    IDragHandler
{
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    public float DropDistance = 170f;
    public float ReturnSpeed = 1.5f;
    private float _t = 1.0f;
    private bool _returnBack;
    private bool _newCardFlag = false;
    private bool _dragClick = false; // click false, drag true
    private bool _equipped = false;
    CardMoverEraser[] kortit;

    void Start()
    {
        _startPosition = transform.position;
        kortit = FindObjectsOfType(typeof(CardMoverEraser)) as CardMoverEraser[];
        foreach (CardMoverEraser kortti in kortit)
        {
            kortti.WildCardAppears();
        };
        string tempString = gameObject.name.Substring(gameObject.name.Length - 1, 1);
        int tempInt = int.Parse(tempString);
        _equipped = transform.parent.GetComponent<DeckScript>().EquipArrayCheck(tempInt);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_dragClick == false)
        {
            equip();
            // Katsoo gameobjectin nimestä viimeisen numeron
            string tempString = gameObject.name.Substring(gameObject.name.Length - 1, 1);
            int tempInt = int.Parse(tempString);
            // Kertoo inventorylle että laittaa käteen oikean esineen
            if(_equipped)   
            
            GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.EquipItem(tempInt);
        }
        _dragClick = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // hover stuff
    }

    // korttien transformit on lokattu =( // fixered
    public void OnPointerUp(PointerEventData eventData)
    {
        //print(Vector2.Distance((Vector2)transform.position, _startPosition) );
        float dis = Vector2.Distance(transform.position, _startPosition);

        //if (Vector2.Distance((Vector2)transform.position, _startPosition) >= DropDistance)
        if (transform.position.y >= DropDistance)
        {
            // Poistaa kortin deckistä
            // Katsoo gameobjectin nimestä viimeisen numeron
            GetComponentInParent<DeckScript>().dragFalse();
            string tempString = gameObject.name.Substring(gameObject.name.Length - 1, 1);
            int tempInt = int.Parse(tempString);
            // Kertoo inventorylle että pudottaa oikean esineen
            GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.DropItem(tempInt);
            // Kertoo deckscriptille että hävittää deckistä oikean kortin
            GameObject.Find("Deck1").GetComponent<DeckScript>().lastBrokenWeapon(tempInt);
        }
        else
        {
            // Palauttaa kortin deckiin
            _endPosition = eventData.position;

            if (_dragClick == true)
            {
                LoadOpenPosition();
                _returnBack = true;
            }
            //GetComponentInParent<DeckScript>().dragFalse();
        }
    }



    void Update()
    {
        if (_newCardFlag)
        {
            _startPosition = transform.position;
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
                GetComponentInParent<DeckScript>().dragFalse();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragClick = true;
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
        _startPosition = tempVec + gameObject.transform.position;
    }

    public void ApplyOpenPosition()
    {
        kortit = FindObjectsOfType(typeof(CardMoverEraser)) as CardMoverEraser[];
        foreach (CardMoverEraser kortti in kortit)
        {
            kortti.LoadOpenPosition();
        };
    }

    private void equip()
    {
        if (_equipped == false)
        {
            Image tempImage = transform.GetChild(1).GetComponent<Image>();
            Color tempColor = tempImage.color;
            kortit = FindObjectsOfType(typeof(CardMoverEraser)) as CardMoverEraser[];
            foreach (CardMoverEraser kortti in kortit)
            {
                kortti.transform.GetChild(1).GetComponent<Image>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
                kortti._equipped = false;
            };
            tempImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.2667f);
            _equipped = true;
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerScript>().UnEquip();
            Image tempImage = transform.GetChild(1).GetComponent<Image>();
            Color tempColor = tempImage.color;
            tempImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
            _equipped = false;
        }

    }
    public bool checkEquip()
    {
        if (_equipped) return true;
        else return false;
    }

    public void AutoEquipFirstItem()
    {
            Image tempImage = transform.GetChild(1).GetComponent<Image>();
            Color tempColor = tempImage.color;
            tempImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.2667f);
            _equipped = true;
    }
}
