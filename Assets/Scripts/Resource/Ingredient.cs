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
        transform.parent = GameObject.Find("luola_tuho").transform;
        if (!_player)
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.GetDropSprite(Type);
        float multiplier = Random.Range(1f, 3f);
        transform.localScale = new Vector3(multiplier, multiplier, multiplier);
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
        // Khää Khää!!
        float z = ZlayerManager.GetZFromY(transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
