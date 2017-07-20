using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

    public GameObject Largeblood;
    public GameObject SmallBlood;
    public GameObject Slow;
    public GameObject CastEffect;
    public GameObject Explosion;
    public static ParticleSpawner instance;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
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
    public void CastSpell(GameObject father)
    {
        var ss = Instantiate(CastEffect);
        ss.GetComponent<castingBuff>().init(father);
    }
    public void SpawnExplosion(Vector2 position,float time = 3f)
    {
        var ss = Instantiate(Explosion);
        ss.GetComponent<SecretExplosion>().init(position,time);
    }
}
