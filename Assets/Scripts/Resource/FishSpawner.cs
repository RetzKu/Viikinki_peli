using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject FishGameObject;
    public GameObject _player;
    public float SpawnRate;
    public float MaxRange = 4f;

    private TileMap _tilemap;

    public GameObject WaterEffect;

    void Start()
    {
        _player     = GameObject.FindWithTag("Player");
        _tilemap    = FindObjectOfType<TileMap>();
        WaterEffect = FishGameObject.GetComponent<Fish>()._waterEffect;
    }

    void Update()
    {
        Vector2 randDir = Random.insideUnitCircle;
        float distance = Random.Range(0f, MaxRange);
        Vector2 pos = (Vector2)transform.position + randDir * distance;
        TileType type = _tilemap.GetTile(pos);

        if (type == TileType.Water || type == TileType.DeepWater)
        {
            // spawn!
            GameObject tileGo = _tilemap.GetGo(pos);
            SpawnFish(tileGo.transform.position);
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnFish(transform.position + new Vector3(0f, 2f));
        }

    }

    private void SpawnFish(Vector3 position)
    {
        var go = Instantiate(FishGameObject);
        go.transform.position = position;
        go.GetComponent<Fish>().Init();
        go.GetComponent<Fish>().Launch((new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized) * 1000f);
    }
}
