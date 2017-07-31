using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pateDebug : MonoBehaviour
{

    // Use this for initialization
    Rigidbody2D body;
    public GameObject door;

    private TileMap tilemap;
    private bool _tilemapActive = true;

    void Start()
    {
        tilemap = FindObjectOfType<TileMap>();
    }

    void Update()
    {


        if (Input.GetKeyDown("o") && _tilemapActive)
        {
            // GetComponent<ScreenShake>().Shake();
            //print("shake dat booty");
            //ParticleSpawner.instance.SpawnFireExplosion(Camera.main.ScreenToWorldPoint(Input.mousePosition),Random.Range(1f,2f));
            //ParticleSpawner.instance.CastSpell(this.gameObject);

            if (_tilemapActive)
            {
                tilemap.DisableTileMap();
                door.GetComponent<door>().Activate();
            }
            else
            {
                tilemap.EnableTileMap();
            }
            _tilemapActive = !_tilemapActive;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            MapGenerator gen = MapGenerator.Instance;
            var go = GameObject.FindWithTag("SpriteController");
            go.GetComponent<TileSpriteController>().SetTileSprites(gen.Width - 1, gen.Height - 1, gen, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
        }
    }
}
