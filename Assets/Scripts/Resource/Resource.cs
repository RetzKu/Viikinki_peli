using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: muillekin tyypeille prefab enumi tallenuksen onnistumiseen
public enum ResourceType
{
    Stone,
    Tree,
}

public abstract class Resource : MonoBehaviour
{
    protected static readonly float TileWidth = 1f;
    public int Hp = 100;
    public float defaultFibrateTimer = 0.1f;
    public float deathTimer = 4f;
    public ResourceType type;

    public virtual void Hit(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            GetComponent<DropScript>().Drop(); // Ehkä static dropper, jossain vaiheessa
            OnDead();
        }
        else
        {
            StartCoroutine(Fibrate(defaultFibrateTimer, TileWidth / 100f)); // general vibrate
        }
    }

    public abstract void OnDead();
    public abstract void Init();


// yleiset efektit, joita voi käyttää
 #region Effects
    protected IEnumerator Fibrate(float seconds, float fibrationRange)  // TODO: MIETI iteraatiot oikein perf
    {
        int iterations = 45;
        float waitTime = seconds / iterations;

        Vector3 startingPosition = transform.position;

        for (int i = 0; i < iterations; i++)
        {
            if (i % 2 == 0) 
            {
                transform.Translate(Random.Range(-fibrationRange, fibrationRange), Random.Range(-fibrationRange, fibrationRange), 0f);
            }
            else
            {
                transform.Translate(Random.Range(-fibrationRange, fibrationRange), Random.Range(-fibrationRange, fibrationRange), 0f);
            }
            yield return new WaitForSeconds(waitTime);
        }
        transform.position = startingPosition;
    }

    protected IEnumerator StartDropTimer()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(deathTimer);

        ObjectPool.instance.PoolObject(this.gameObject);
    }

    // TODO: tee resuille oma partikkelie effect
    private IEnumerator EmitParticles(Vector3 hitPosition, float intensity, float seconds)
    {
        //EffectEmitters.transform.position = hitPosition; // HIT KOHTAAN halutaan
        //var particleSystem = EffectEmitters.GetComponent<ParticleSystem>();
        //particleSystem.Play();

        yield return new WaitForSeconds(seconds);   // Voi hoitaa kokonaan ParticleSystemissä
        //particleSystem.Stop();
    }
#endregion

    public static string GetResourcePrefabName(ResourceType keyvaluepairValue)
    {
        switch (keyvaluepairValue)
        {
            case ResourceType.Stone: return "Stone";
            case ResourceType.Tree: return "SpawnableTree";
            default:
                Debug.LogWarning("RESOURCE TO PREFAB MAPPIN NOT FOUND IN RESOURCE.cs");
                return "null";
        }
    }
}
