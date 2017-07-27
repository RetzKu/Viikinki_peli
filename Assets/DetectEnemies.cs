using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemies : MonoBehaviour
{

    private Rigidbody2D body;
    public float aggroDist = 5.0f;
    public float slowDown = 0.2f;
    LayerMask mask = new LayerMask();
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mask = LayerMask.GetMask("Enemy");
        StartCoroutine(aggro());
        StartCoroutine(slow());

    }

    IEnumerator aggro()
    {
        for (;;)
        {
            var aggroArray = Physics2D.OverlapCircleAll(body.position, aggroDist, mask); // , mask);
            for (int i = 0; i < aggroArray.Length; i++)
            {
                //print("BERZERG");
                // tähän check että ei ole minkään takana
                aggroArray[i].transform.root.GetComponent<generalAi>().agro = true;
            }
        yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator slow()
    {
        for (;;)
        {
            var slowArray = Physics2D.OverlapCircleAll(body.position, slowDown, mask); // , mask);
            if (slowArray.Length > 0)
            {
                GetComponent<Movement>().Started = true;
                GetComponent<Movement>().Slowed = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public Vector2 getPosition()
    {
        return body.position;
    }
}
