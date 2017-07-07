using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    // Stone,
    Wood,
    Pine,
    Bone,
    Hemp,
    Tooth,
    BearFur,
    Food,
    Feather,
    Max
}

[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour
{
    public IngredientType Type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Poimi
            CraftingManager.Instance.AddToInventory(this.gameObject);
            GetComponent<BoxCollider2D>().enabled = false; // ei useita
        }
    }
}
