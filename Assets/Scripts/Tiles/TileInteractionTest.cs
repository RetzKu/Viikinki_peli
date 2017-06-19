using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractionTest : MonoBehaviour
{
    public float maxReachDistance = 2f;
    public TileMap tilemap;
    private Perlin perlinGenerator;
    Vector2 mousePos;

    private bool _running = false;

    void Start()
    {
        tilemap = FindObjectOfType<TileMap>();
        perlinGenerator = FindObjectOfType<Perlin>();
        _running = true;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (Input.GetMouseButtonDown(0))
        {
            Vector2 reach = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

            if (reach.magnitude < maxReachDistance)
            {
                var go = tilemap.GetTileGameObject(mousePos.x, mousePos.y);
                go.GetComponent<Renderer>().material.color = Color.grey;
                go.GetComponent<Collider2D>().enabled = true;
            }
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 reach = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

            if (reach.magnitude < maxReachDistance)
            {
                var go = tilemap.GetTileGameObject(mousePos.x, mousePos.y);
                var tile = tilemap.GetTile(mousePos.x, mousePos.y);

                go.GetComponent<Renderer>().material.color = perlinGenerator.BiomeToColor(tile);
                go.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_running)
        {
            Gizmos.DrawWireCube(new Vector3(mousePos.x, mousePos.y, 0), new Vector3(1f, 1f, 0));
            var tile = tilemap.GetTileGameObject(mousePos.x, mousePos.y);
            Gizmos.DrawWireCube(new Vector3(tile.transform.position.x, tile.transform.position.y, 0), new Vector3(1f, 1f, 0));
        }
    }
}
