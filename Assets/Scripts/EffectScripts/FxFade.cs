using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxFade : MonoBehaviour
{
    private float startTime;
    public float Duration;
    private SpriteRenderer FxSprite;

    void Start()
    {
        startTime = Time.time;
        FxSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float t = (Time.time - startTime) / Duration;
        FxSprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0.1f, 1, t));
    }

}
