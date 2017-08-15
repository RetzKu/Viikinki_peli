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

    public void LaunchArrow(Sprite sprite, Vector3 lookAt, Color speedColor)
    {
        StartCoroutine(LaunchArrowEffect(sprite, lookAt, transform.position, speedColor));
    }

    IEnumerator LaunchArrowEffect(Sprite sprite, Vector3 lookAt, Vector3 from, Color speedColor)
    {
        GameObject go = new GameObject("arrow :D");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;

        go.transform.position = this.transform.position;
        go.transform.up = lookAt - from;


        // fade
        for (int i = 0; i < 40; i++)
        {
            go.transform.position += new Vector3(0f, 4f * Time.deltaTime, 0f);
            go.transform.up = lookAt - go.transform.position;
            // renderer.color = Color.Lerp(Color.white, speedColor, (float) i / 40); // XD

            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(attack(go, lookAt, speedColor));
        // Destroy(go);
    }

    IEnumerator attack(GameObject go, Vector3 lookAt, Color speedColor)
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

    public void LaunchAoeFader(Sprite sprite, AoeEffectData data)
    {
        StartCoroutine(LaunchFader(sprite, data));
    }

    void OnDrawGizmos()
    {
        if (_aoeEffectRunning)
        {
            Gizmos.DrawWireSphere(_aoeStartPoint, _aoeEffectRadius);
        }
    }

    IEnumerator LaunchFader(Sprite sprite, AoeEffectData buffData)
    {
        
        GameObject go = new GameObject("Fader :D");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;

        go.transform.position = this.transform.position;
        _aoeStartPoint = go.transform.position; // debgu

        int iterationForEffect = buffData.Frames;
        float growAmountPerFrame = (buffData.EndScale - buffData.StartScale) / buffData.Frames;
        go.transform.localScale = new Vector3(buffData.StartScale, buffData.StartScale);

        float t = 0;
        float tIncrement = 1f / iterationForEffect;

        go.transform.Translate(buffData.StartOffset);

        for (int i = 0; i < iterationForEffect; i++)
        {
            go.transform.localScale = new Vector3(go.transform.localScale.x + growAmountPerFrame, go.transform.localScale.y + growAmountPerFrame);
            go.transform.Rotate(new Vector3(0, 0, buffData.TotalRotation / iterationForEffect));

            if (buffData.FollowsPlayer)
            {
                print(transform.position);
                go.transform.position = transform.position;
            }

            renderer.color = Color.Lerp(Color.white, buffData.EndColor, t);
            t += tIncrement;
            yield return null;
        }

        Destroy(go);
        _aoeEffectRunning = false; //debug
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

        float t = 0;
        float tIncrement = 1f / iterationForEffect;

        go.transform.Translate(buffData.StartOffset);

        for (int i = 0; i < iterationForEffect; i++)
        {
            go.transform.localScale = new Vector3(go.transform.localScale.x + growAmountPerFrame, go.transform.localScale.y + growAmountPerFrame);
            // go.transform.Translate(buffData.MovementDir * buffData.Speed * Time.deltaTime);

            if (buffData.FollowsPlayer)
            {
                print(transform.position);
                go.transform.position = transform.position;
            }

            go.transform.Rotate(new Vector3(0, 0, buffData.TotalRotation / iterationForEffect));

            if (iterationForEffect % 3 == 0 || iterationForEffect <= 1) // mikä on hyvä applytys aika
            {
                float radius = go.transform.localScale.x / 2f;
                _aoeEffectRadius = radius; // debug
                _aoeStartPoint = go.transform.position;
                var colliders = Physics2D.CircleCastAll(go.transform.position, radius, new Vector2(0, 0), 0, mask);
                if (colliders != null)
                {
                    foreach (var collider in colliders)
                    {
                        buffData.BuffToApply.Apply(collider.transform.gameObject);
                    }
                }
            }

            renderer.color = Color.Lerp(Color.white, buffData.EndColor, t);
            t += tIncrement;
            yield return null;
        }

        Destroy(go);

        _aoeEffectRunning = false; //debug
        if (buffData.AfterEffect != null)
        {
            _afterEffects.Add(buffData.AfterEffect);
        }
    }

    public void LaunchPlayerSingleTargetBuff(Buff buff)
    {
        buff.Apply(GameObject.FindWithTag("Player"));
    }
}
