﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeShaderController : MonoBehaviour
{
    private TileMap _tileMap;
    private bool _running = false;

    void Start()
    {
        var tilemapGo = GameObject.FindWithTag("Tilemap");
        if (tilemapGo == null)
        {
            Debug.LogError("No tilemap or tilemap tag isn't Tilemap");
        }
        else
        {
            _tileMap = tilemapGo.GetComponent<TileMap>();
        }
        _running = true;
    }

    void OnDrawGizmos()
    {
        if (_running)
        {
            // ylä
            Gizmos.DrawWireCube(_tileMap.GetGo(transform.position + new Vector3(1.0f, 1.0f, 0f)).transform.position, Vector3.one);
            Gizmos.DrawWireCube(_tileMap.GetGo(transform.position + new Vector3(0f, 1.0f, 0f)).transform.position, Vector3.one);

            // ala
            // Gizmos.DrawWireCube(_tileMap.GetGo(transform.position + new Vector3(0f, 0f, 0f)).transform.position, Vector3.one);
            // Gizmos.DrawWireCube(_tileMap.GetGo(transform.position + new Vector3(1.0f, 0f, 0f)).transform.position, Vector3.one);
        }
    }

    void Update()
    {
        // onko kaikki pakollisia
        TryToSetShader(new Vector3(1.0f, 1.0f, 0f));
        TryToSetShader(new Vector3(1.0f, 0f, 1.0f));

        TryToSetShader(new Vector3(0f, 0f, 0f));
        TryToSetShader(new Vector3(1f, 0f, 0f));
    }

    void TryToSetShader(Vector3 offset)
    {
        var resourceOnTile = _tileMap.GetTileOnTileGameObject(transform.position + offset);

        if (resourceOnTile != null)
        {

            if (ResourceManager.IsBehindable(resourceOnTile.GetComponent<Resource>().type))
            {
                Resource resource = resourceOnTile.GetComponent<Resource>();

                if (resource.transform.position.z < transform.position.z)
                {
                    resource.SetOcculuderShader();
                }
                else
                {
                    resource.SetNormalShader();
                }
            }
        }
    }
}
