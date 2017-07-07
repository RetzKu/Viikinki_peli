using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : Rune
{
    public GameObject Prefab;
    public List<Cost> Materials;

    public override void init(GameObject owner)
    {
        // Launcheriin, jokin cool effect, joka tulee kun craftaat + ääni
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
