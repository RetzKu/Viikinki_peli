using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemies : MonoBehaviour
{

    private Rigidbody2D body;
    public float aggroDist = 5.0f;
    public float slowDown = 0.2f;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        var aggroArray = Physics2D.OverlapCircleAll(body.position, aggroDist, mask); // , mask);
        for (int i = 0; i < aggroArray.Length; i++)
        {

            //print("BERZERG");
            // tähän check että ei ole minkään takana
            aggroArray[i].transform.root.GetComponent<generalAi>().agro = true;
        }
        var slowArray = Physics2D.OverlapCircleAll(body.position, slowDown, mask); // , mask);
        if(slowArray.Length > 0)
        {
            GetComponent<Movement>().lerpUp = false;
        }
        else
        {
            GetComponent<Movement>().lerpUp = true;
        }
    }
    public Vector2 getPosition()
    {
        return body.position;
    }
}
