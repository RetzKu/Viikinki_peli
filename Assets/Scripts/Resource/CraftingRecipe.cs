using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : Rune
{
    public GameObject Prefab;

    [Header("No need to Use Indices by hand")]
    public List<Cost> Materials;

    public override void init(GameObject owner)
    {
        // Launcheriin, jokin cool effect, joka tulee kun craftaat + ääni


        // Gettaa order -> laita indices oikein
        Indices = new Vec2[Materials.Count];
        for (int i = 0; i < Materials.Count; i++)
        {
            Vec2 vec = CraftingManager.Instance.GetCraftingIndexes(Materials[i].Type);
            Indices[i] = vec;
        }
    }

    public override void Fire()
    {
        if (CraftingManager.Instance.TryToCraftItem(Materials, Prefab))
        {
            // TODO: CRAFTAUS ONNISTUI
            Debug.Log("Kirves!");
        }
        else
        {
            // TODO: CRAFTAUS FAILASI

        }
        // TODO: LAUNCHER, SOUND
    }
}

[System.Serializable]
public struct Cost
{
    public IngredientType Type;
    public int Count;

    public Cost(IngredientType type, int count)
    {
        Type = type;
        Count = count;
    }
}
