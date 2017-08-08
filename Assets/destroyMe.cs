using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyMe : MonoBehaviour {
    float lifeTime = 2.7f;
    float counter = 0f;
    bool visited = false;
    bool inited = false;
    // Use this for initialization

    Vector2 direction;
    float angle;

	void Awake () {
        GetComponent<ParticleSystem>().Pause();
        //GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[] 
        //{ new ParticleSystem.Burst(0.0f, 50, 50)/*, new ParticleSystem.Burst(1.0f, 10, 20) */});

    }

    public void initParticle(Vector2 from,Vector2 spawn)
    {
        GetComponent<ParticleSystem>().Play();
        transform.position = spawn;
        direction = spawn - from;
        inited = true;

        direction.Normalize();
        angle = Mathf.Atan2(-direction.y, -direction.x);
        //print(Quaternion.Euler(angle * Mathf.Rad2Deg, -90, -90));// Quaternion.Euler(angle, -90, -90););
        transform.rotation = Quaternion.Euler(angle * Mathf.Rad2Deg, -90, -90);// Quaternion.Euler(angle, -90, -90);
    }
	void Update()
    {
        if (inited)
        {
            counter += Time.deltaTime;
            if(counter > lifeTime && !visited)
            {
                Debug.Log("destroy blood");
                GetComponent<ParticleSystem>().Pause();
                Destroy(this.gameObject, 5f);
                visited = true;
            }
            
        }
        
    }


}
