using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Myöhemmin hoitamaan runejen cd:t jne...
public class RuneEffectLauncher : MonoBehaviour
{
    //private ParticleSystem _particleSystem;
    private bool _aoeEffectRunning = false;
    private float _aoeEffectRadius = 0f;
    private Vector2 _aoeStartPoint = new Vector2(0f, 0f);
    private List<Rune> _afterEffects = new List<Rune>(4);

    void Start()
    {
        //_particleSystem =  GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_afterEffects.Count != 0) // testi mielessä
        {
            _afterEffects[_afterEffects.Count - 1].init(this.gameObject); // TODO: tietyille runeille ei toimi korjaa kun tulee single targets
            _afterEffects[_afterEffects.Count - 1].Fire(); // nyt ollaan aivan huluja / rekursio params aoedataan XD
            _afterEffects.RemoveAt(_afterEffects.Count - 1);
        }
    }

    public void LaunchArrow(Sprite sprite, Vector3 lookAt)
    {
        StartCoroutine(LaunchArrowEffect(sprite, lookAt, transform.position));
    }

    IEnumerator LaunchArrowEffect(Sprite sprite, Vector3 lookAt, Vector3 from)
    {
        GameObject go = new GameObject("arrow :D");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;

        go.transform.position = this.transform.position;
        go.transform.localScale = new Vector3(5f, 5f);

        go.transform.up = lookAt - from;

        // fade
        for (int i = 0; i < 40; i++)
        {
            // go.transform.Translate(0f, 0.2f * Time.deltaTime, 0f); 
            go.transform.position += new Vector3(0f, 10f * Time.deltaTime, 0f);
            go.transform.up = lookAt - go.transform.position;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(attack(go, lookAt));
        // Destroy(go);
    }

    IEnumerator attack(GameObject go, Vector3 lookAt)
    {
        for (int i = 0; i < 50; i++)
        {
            go.transform.Translate(new Vector3(0f, 0.4f, 0f));
            go.transform.up = lookAt - go.transform.position;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(go);
    }

    public void FireAoeEffect(Sprite sprite, AoeEffectData data, LayerMask mask)
    {
        StartCoroutine(LaunchEffect(sprite, data, mask));
    }

    void OnDrawGizmos()
    {
        if (_aoeEffectRunning)
        {
            Gizmos.DrawWireSphere(_aoeStartPoint, _aoeEffectRadius);
        }
    }

    // private readonly int IterationForEffect = 60;
    IEnumerator LaunchEffect(Sprite sprite, AoeEffectData buffData, LayerMask mask)
    {
        _aoeEffectRunning = true; //debug

        GameObject go = new GameObject("Jää runiiddi :D / aoe effect");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;

        go.transform.position = this.transform.position;
        _aoeStartPoint = go.transform.position; // debgu

        int iterationForEffect = buffData.Frames;
        float growAmountPerFrame = (buffData.EndScale - buffData.StartScale) / buffData.Frames;
        go.transform.localScale = new Vector3(buffData.StartScale, buffData.StartScale);

        for (int i = 0; i < iterationForEffect; i++)
        {
            go.transform.localScale = new Vector3(go.transform.localScale.x + growAmountPerFrame, go.transform.localScale.y + growAmountPerFrame);
            go.transform.Translate(buffData.MovementDir * buffData.Speed * Time.deltaTime);
            go.transform.Rotate(new Vector3(0, 0, buffData.TotalRotation / iterationForEffect));

            if (iterationForEffect % 3 == 0) // mikä on hyvä applytys aika
            {
                float radius = go.transform.localScale.x / 2f;
                _aoeEffectRadius = radius; // debug
                _aoeStartPoint = go.transform.position;
                var colliders = Physics2D.CircleCastAll(go.transform.position, radius, new Vector2(0, 0), 0, mask);
                foreach (var collider in colliders)
                {
                    buffData.BuffToApply.Apply(collider.transform.gameObject);
                }
            }
            yield return null;
        }

        // TODO: effectin fadetus !
        Destroy(go);

        _aoeEffectRunning = false; //debug
        if (buffData.AfterEffect != null)
        {
            _afterEffects.Add(buffData.AfterEffect);
        }
    }
}
