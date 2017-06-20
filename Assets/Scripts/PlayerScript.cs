using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    internal List<ItemScript> invetory_data;
    public int InventorySize;

    // private List<Item_Values> id_in_range;
    //     private Item_Values closest_item;

    private bool interraction_cd = false;

    Vector3 startPoint;
    Vector3 endPoint;

    public bool running = true;

    public GameObject text_object;

    void Start()
    {
        invetory_data = new List<ItemScript>(InventorySize);
        //id_in_range = new List<Item_Values>(100);
    }

    void Update()
    {

        // KeyCommands();
        tmpswing();
        OnTriggerEnter2D(GetComponent<Collider2D>());
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



        if (Input.GetKey(KeyCode.Mouse0) == true)
        {
            if (clickPosition.x < 0.0f & GameObject.Find("Equip").transform.childCount >= 1)

            {
                //GameObject.FindGameObjectWithTag("item2").GetComponent<SpriteRenderer>().flipX = true;
                //Debug.Log("loopinsisaanpaastiin1");
                GameObject.FindGameObjectWithTag("item2").transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            }

            else if (clickPosition.x > 0.0f & GameObject.Find("Equip").transform.childCount >= 1)
            {
                //GameObject.FindGameObjectWithTag("item2").GetComponent<SpriteRenderer>().flipX = false;
                //Debug.Log("loopinsisaanpaastiin2");
                GameObject.FindGameObjectWithTag("item2").transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            }
        }

        else if (GameObject.Find("Equip").transform.childCount >= 1)
        {
            GameObject.FindGameObjectWithTag("item2").transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
        
    }
    void OnDrawGizmos()
    {
        if (running == true)
        {
            Gizmos.DrawLine(startPoint, endPoint); // piirretään viiva visualisoimaan toimivuutta
        }
    }

    void OnTriggerEnter2D(Collider2D Trig)
    {
        if (Trig.gameObject.tag == "item_maassa")
        {
            Trig.gameObject.tag = "item_inventoryssa";
            Instantiate(Trig.gameObject, GameObject.Find("Inventory").transform);
            Destroy(Trig.gameObject);
        }

        if (Trig.gameObject.tag == "puu")
        {
            Debug.Log("BONK");
            Trig.GetComponent<TreeHP>().hp -= 25;
        }

        //Debug.Log("Toimii");

    }

    void OnTriggerExit2D(Collider2D Trig)
    {
        //Debug.Log("Ulos");
        
    }

}