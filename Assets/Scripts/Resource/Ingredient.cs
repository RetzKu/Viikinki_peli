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

        var go = GameObject.Find("luola_tuho").transform;
        if (go != null)
        {
            transform.parent = go.transform;
        }

        if (!_player)
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.GetDropSprite(Type);

        transform.localScale = new Vector3(3f, 3f, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.localScale = new Vector3(5f, 5f, 5f);
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
