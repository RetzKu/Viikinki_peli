using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sampler : MonoBehaviour
{
    private PoissonDiscSampler sampler;
    public float w, h, r;

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Texture2D texture = new Texture2D((int)w, (int)h);
            sampler = new PoissonDiscSampler(w, h, r);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            int count = 0;
            foreach (Vector2 sample in sampler.Samples())
            {
                texture.SetPixel((int)sample.x, (int)sample.y, Color.black);
                count++;
            }

            texture.Apply();
            renderer.material.mainTexture = texture;
        }
    }
}
