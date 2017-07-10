using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHell : MonoBehaviour
{


	void Start ()
    {
        		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject bullet = ObjectPool.instance.GetObjectForType("Bullet", true);

            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                var bul = bullet.GetComponent<Bullet>();

                bul.Speed = 2;
                bul.velocity = Random.insideUnitCircle;
            }
        }
	}
}
