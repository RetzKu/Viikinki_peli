using System;
using UnityEngine;

// viimesen päivän tiili animaattori XD:
public class TileAnime : MonoBehaviour
{

    public static float ChangeTime = 0.7f;
    public static Sprite[] deep;
    public static Sprite[] normal;

    private static int _index;
    private static float _timer;

    private int _ownIndex = 0;
    private SpriteRenderer _renderer;

    private Sprite[] _sprites;


    public static void InitAnime(Sprite[] deepWater, Sprite[] normalWater)
    {
        _index = 0;
        deep   = deepWater;
        normal = normalWater;
    }

    public void SetSprites(TileType animeType)
    {
        switch (animeType)
        {
            case TileType.Water:
                _sprites = normal;
                break;
            case TileType.DeepWater:
                _sprites = deep;
                break;
            default:
                Debug.LogWarning("this tile (" + animeType + ") doest't have animation TileAnime.cs");
                break;
        }
    }

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public static void UpdateTiles()
    {
        _timer += Time.deltaTime;
        if (_timer > ChangeTime)
        {
            _timer = 0f;
            _index++;
            if (_index == 8) //Sprites.Length)
            {
                _index = 0;
            }
        }
    }

    void Update()
    {
        if (_index != _ownIndex)
            _renderer.sprite = _sprites[_index];
    }

    public void Continue()
    {
        // _renderer.sprite = _sprites[_index];
    }
}
