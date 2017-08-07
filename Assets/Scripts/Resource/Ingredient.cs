using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    // Stone,
    Wood,
    Bone,
    Hemp,
    Tooth,
    BearFur,
    Stone,
    Feather,
    SeaWeed,
    FishScale,
    Max,
}

[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour
{
    public IngredientType Type;
    private static Transform _player;

    void Start()
    {
        if (!_player)
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Poimi
            CraftingManager.Instance.AddToInventory(this.gameObject);
            GetComponent<BoxCollider2D>().enabled = false; // ei useita
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x - _player.transform.position.x) > Chunk.CHUNK_SIZE * 2 || 
            Mathf.Abs(transform.position.y - _player.transform.position.y) > Chunk.CHUNK_SIZE * 2   )
        {
            Destroy(this.gameObject);
        }
    }
}
