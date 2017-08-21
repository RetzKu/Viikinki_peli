using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

    public GameObject Largeblood;
    public GameObject SmallBlood;
    public GameObject Slow;
    public GameObject fireBuff;
    public GameObject CastEffect;
    public GameObject Explosion;
    public GameObject fireExp;
    public GameObject rockExp;
    public GameObject DyingEffect;
    public static ParticleSpawner instance;
    //public List<GameObject> bloods = new List<GameObject>();

    private Transform _parent;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        _parent = new GameObject("luola_tuho").GetComponent<Transform>();
    }
    public void SpawnLargeBlood(Vector2 from,Vector2 where)
    {
        var newBlood = Instantiate(Largeblood, new Vector2(0, 0), Quaternion.identity);
        newBlood.GetComponent<destroyMe>().initParticle(from, where);
        newBlood.transform.parent = _parent;
        //bloods.Add(newBlood);
    }
    public void SpawSmallBlood(Vector2 from, Vector2 where)
    {
        var newBlood = Instantiate(SmallBlood, new Vector2(0, 0), Quaternion.identity);
        newBlood.GetComponent<destroyMe>().initParticle(from, where);
        newBlood.transform.parent = _parent;
    }
    public void SpawSlow(GameObject father,float time)
    {
        var ss = Instantiate(Slow);
        ss.GetComponent<buffParticle>().init(father, time);
    }
    public void SpawFireEffect(GameObject father, float time)
    {
        var ss = Instantiate(fireBuff/*, new Vector2(0, 0), Quaternion.Euler(-90,0,0)*/);
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
    public void SpawnFireExplosion(Vector2 position, float time = 3f)
    {
        var ss = Instantiate(fireExp);
        ss.GetComponent<SecretExplosion>().init(position, time);
    }
    public void SpawnRockExplosion(Vector2 position, float time = 3f)
    {
        var ss = Instantiate(rockExp);
        ss.GetComponent<SecretExplosion>().init(position, time);
    }
    public void SpawnDyingEffect(Vector2 position)
    {
        var ss = Instantiate(DyingEffect);
        ss.transform.position = position + new Vector2(0,-0.2f);
        Destroy(ss, 1f);
    }
    public void destroybloods()
    {
        Destroy(_parent.gameObject);
        _parent = new GameObject("luola_tuho").GetComponent<Transform>();
    }
}
