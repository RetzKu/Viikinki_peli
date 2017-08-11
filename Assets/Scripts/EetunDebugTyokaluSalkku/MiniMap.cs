using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Texture2D _wholeMap;
    private bool _initted = false;
    private SpriteRenderer _renderer;
    private Transform _playerTransform;
    public float w, h, x, y;

    public MeshRenderer Mesh;

    void Start()
    {
        GameObject rendererGo = new GameObject("Minimap renderer");
        _renderer = rendererGo.AddComponent<SpriteRenderer>();
        rendererGo.transform.parent = transform;
        _playerTransform = GameObject.FindWithTag("Player").transform;

    }

    // nope
    void Update()
    {
        // 0.17f
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GenMap();
        //}

        // show part & move
        //if (_initted)
        //{
        //    _renderer.sprite.rect.Set(x, y, w, h);
        //}

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    _renderer.sprite.rect.Set(x, y, w, h);
        //}

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    _renderer.sprite.textureRect.Set(x, y, w, h);
        //}
    }

    private void GenMap()
    {
        if (!_initted)
        {
            var go = GetComponent<Perlin>();
            _wholeMap = go.GenMiniMap(1000, 1000);
            _initted = true;
            _renderer.material.mainTexture = _wholeMap;
            _renderer.sprite = Sprite.Create(_wholeMap, new Rect(0f, 0f, 1000f, 1000f), Vector2.zero);

            Mesh.material.mainTexture = _wholeMap;
            // _renderer.sprite.textureRect.Set(0f, 0f, 50f, 50f);

        }
    }
}
