using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

    public GameObject Largeblood;
    public GameObject SmallBlood;
    public GameObject Slow;
    public static ParticleSpawner inctance;
    void Start()
    {
        if(inctance == null)
        {
            inctance = this;
        }
    }
    public void SpawnLargeBlood(Vector2 from,Vector2 where)
    {
        var newBlood = Instantiate(Largeblood, new Vector2(0, 0), Quaternion.identity);
        newBlood.GetComponent<destroyMe>().initParticle(from, where);
    }
    public void SpawSmallBlood(Vector2 from, Vector2 where)
    {
        var newBlood = Instantiate(SmallBlood, new Vector2(0, 0), Quaternion.identity);
        newBlood.GetComponent<destroyMe>().initParticle(from, where);
    }
    public void SpawSlow(GameObject father,float time)
    {
        var ss = Instantiate(Slow);
        ss.GetComponent<buffParticle>().init(father, time);
    }

}
