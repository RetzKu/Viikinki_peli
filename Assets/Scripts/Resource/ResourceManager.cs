﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Material NormalSpriteMaterial;
    public Material OcculuderSpriteMaterial;


    [Header("Initoidaan Awakessa")]
    public static ResourceManager Instance = null;
    private string[] resourceTypeLookupTable = new string[(int)ResourceType.Max];

    private readonly int resourceTypeToTrunk = (int) (ResourceType.t_trunkEnd - ResourceType.t_trunkStart);
    public List<DropScript.Drops> CorpseDrops;
    private static Sprite[] _runeSprites;

    void Awake()
    {
        Instance = this;

        // Inisoi rakkaat ruumit
        Corpse.CorpseSprites = Resources.LoadAll<Sprite>("WorldObject/corpses");

        Tree._treeShadows = Resources.LoadAll<Sprite>("WorldObject/shadows");

        _runeSprites = Resources.LoadAll<Sprite>("WorldObject/RunePillars/runepillars");

        initDropSprites();
    }

    void Start()
    {
        for (int i = 0; i < (int)ResourceType.Max; i++)
        {
            resourceTypeLookupTable[i] = ((ResourceType) i).ToString();
        }

        // puun kannot
        for (int i = (int) ResourceType.t_trunkStart + 1; i < (int) ResourceType.t_trunkEnd; i++) // <=
        {
            string str = ((ResourceType) i).ToString();
            resourceTypeLookupTable[i] = str.Substring(0, str.Length - 6); // _ t r u n k == 6
        }

    }

    public string GetResourceTypeName(ResourceType type)
    {
        return resourceTypeLookupTable[(int) type];
    }

    public ResourceType GetTrunk(ResourceType type)
    {
        if (IsTrunkType(type))
        {
            return type;
        }
        return type + resourceTypeToTrunk;
    }


    public bool IsTrunkType(ResourceType type)
    {
        return (ResourceType.t_trunkStart < type && ResourceType.t_trunkEnd > type);
    }

    public static bool IsAliveTree(ResourceType type)
    {
        return (ResourceType.t_birch0 <= type && ResourceType.t_spruce1 >= type);
    }

    public List<DropScript.Drops> GetCorpseDrops()
    {
        return CorpseDrops;
    }

    // valitan
    // shadow_+ num
    public static int TreeToShadow(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.t_pine0:
            case ResourceType.t_pine0_trunk:
            case ResourceType.t_spruce0:
            case ResourceType.t_spruce1:
                return 0;
            case ResourceType.t_birch0:
            case ResourceType.t_birch1:
            case ResourceType.t_birch2:
            case ResourceType.t_lime0:
            case ResourceType.t_lime1:
            case ResourceType.t_lime2:
            case ResourceType.t_willow0:
            case ResourceType.t_willow1:
            case ResourceType.t_willow2:
                return 1;
            case ResourceType.t_birch0_trunk:
            case ResourceType.t_birch1_trunk:
            case ResourceType.t_birch2_trunk:
            case ResourceType.t_lime0_trunk:
            case ResourceType.t_lime1_trunk:
            case ResourceType.t_lime2_trunk:
            case ResourceType.t_willow0_trunk:
            case ResourceType.t_willow1_trunk:
            case ResourceType.t_willow2_trunk:
            case ResourceType.t_spruce0_trunk:
            case ResourceType.t_spruce1_trunk:
                return 2;
            default:
                Debug.LogError("TreeToShader for " + type.ToString() + " missing, ResourceManager.cs");
                return 0;
        }
    }


    public static Sprite GetFallenTreeSprite()
    {
        return Tree._treeShadows[2];
    }

    public static Material GetNormalShader()
    {
        return Instance.NormalSpriteMaterial;
    }

    public static Material GetOcculuderShader()
    {
        return Instance.OcculuderSpriteMaterial;
    }

    public static bool IsBehindable(ResourceType type)
    {
        return ResourceType.t_trunkEnd >= type;
    }


    public Sprite[] DropSprites = new Sprite[9];
    private void initDropSprites()
    {
        DropSprites = Resources.LoadAll<Sprite>("WorldObject/droppedResources");
        if (DropSprites == null)
        {
            Debug.LogError("could't load dropsprites");
        }
    }

    public Sprite GetDropSprite(IngredientType type)
    {
        print(type);
        print(DropSprites.Length);
        return DropSprites[(int)type];
    }
}
