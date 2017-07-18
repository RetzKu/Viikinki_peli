using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

    public GameObject blood;

    public static ParticleSpawner inctance;

    void start()
    {
        if(inctance != null)
        {
            inctance = this;
        }
    }
    public void SpawnBlood(Vector2 from,Vector2 where)
    {
        var newBlood = Instantiate(blood, new Vector2(0, 0), Quaternion.identity);
        newBlood.GetComponent<destroyMe>().initParticle(from, where);
    }
    
}
