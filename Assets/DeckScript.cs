using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public float maximum = 1.0f;
    public float minimum = -1.0f;

    public float x = 262.7f;
    public float y = 92.49f;
    public bool lerp = false;
    public float Speed;
    public float rotMin;
    public float rotMax;
    static float t = 0.0f;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (!lerp)
        {
            transform.GetChild(2).position = new Vector3(x, y, 0f);
        }
        else
        {
            transform.GetChild(2).position = new Vector3(x, Mathf.Lerp(minimum, maximum, t), 0f);

        }
        
        // t = EnemyMovement.map(Input.mousePosition.y, 0f, 300f, 0f, 1f);
        print(t);
        //  t += 0.5f * Time.deltaTime;
        t += Time.deltaTime * Speed;
        transform.GetChild(2).position = Vector3.Lerp(transform.position, transform.position + new Vector3(-100f, 100f, 0f), t);

        
        transform.GetChild(2).rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(rotMin, rotMax, t));



        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
	}
}
