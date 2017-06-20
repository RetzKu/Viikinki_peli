using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    internal List<ItemScript> invetory_data;
    public int InventorySize;

    private bool interraction_cd = false;
    private GameObject InventoryChild;

    Vector3 startPoint;
    Vector3 endPoint;

    public bool running = true;

    private GameObject text_object;


    void Start()
    {
        invetory_data = new List<ItemScript>(InventorySize);
        InventoryChild = gameObject.transform.Find("Inventory").gameObject;
    }

    void Update()
    {
        tmpswing();
    }

    void tmpswing()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, 0.0f); // Pelaajan positio
        Vector3 clickPosition = new Vector3();
        clickPosition.x = Camera.main.ScreenToWorldPoint(mousePos).x - playerPosition.x; // clickPosition on loppupiste - alkupiste
        clickPosition.y = Camera.main.ScreenToWorldPoint(mousePos).y - playerPosition.y;
        clickPosition.z = 0.0f;

        Vector3.Normalize(clickPosition);
        //print(clickPosition);
        startPoint = playerPosition; // Pelaajan positio
        endPoint = Camera.main.ScreenToWorldPoint(mousePos); // Hiiren osoittama kohta

        if (clickPosition.x < 0.0f)
        {
            if (GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX == false)
            {
                //GameObject.Find("Lapio").GetComponent<Transform>().position.x = -0.4; // Väittää että muuttuja olisi constant vaikkei pitäisi olla :c
                GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        if (clickPosition.x > 0.0f)
        {
            if (GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX == true)
            {
                GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) == true)
        {

            GetComponentInChildren<Animator>().SetTrigger("lapioAttack");
            //print(clickPosition);
        }
    }
    void OnDrawGizmos()
    {
        if (running == true)
        {
            Gizmos.DrawLine(startPoint, endPoint); // piirretään viiva visualisoimaan toimivuutta
        }
    }

    public void OnTriggerEnter2D(Collider2D Trig)
    {
        InventoryChild.GetComponent<inventory>().AddItem(Trig.gameObject);
        Debug.Log("debug");
        if (Trig.gameObject.tag == "item_maassa")
        {
            Debug.Log("Toimii");
        }
    }

    void OnTriggerExit2D(Collider2D Trig)
    {
        Debug.Log("Ulos");
        Trig.gameObject.tag = "item_inventoryssa";
        Instantiate(Trig.gameObject, GameObject.Find("Inventory").transform);
        Destroy(Trig.gameObject);
    }

}