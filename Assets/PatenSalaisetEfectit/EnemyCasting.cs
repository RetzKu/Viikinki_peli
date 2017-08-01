using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastTypes
{
    Light,
    Fire,
    Rock
}   

public class EnemyCasting : MonoBehaviour {

    public GameObject player;
    public float castTime;
    public float castCircle;
    private CastTypes _myType = CastTypes.Light;
    public CastTypes myType { set { _myType = value; } }
    float counter = 0;
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter > castTime)
        {
            int castTimes = Random.Range(1, 5);
            for(int i = 0; i < castTimes; i++)
            {
                Vector2 r = Random.insideUnitCircle * Random.Range(0f, castCircle) + (Vector2)player.transform.position;
                castEffect(r);
            }
            counter = 0;
        }

	}

    void castEffect(Vector2 place)
    {
        switch (_myType)
        {
            case CastTypes.Light:
                ParticleSpawner.instance.SpawnExplosion(place);
                break;
            case CastTypes.Fire:
                ParticleSpawner.instance.SpawnFireExplosion(place);
                break;
            case CastTypes.Rock:
                ParticleSpawner.instance.SpawnRockExplosion(place);
                break;
        }
    }


}
