using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Initoidaan Awakessa")]
    public static ResourceManager Instance = null;
    private string[] resourceTypeLookupTable = new string[(int)ResourceType.Max];

    private readonly int resourceTypeToTrunk = (int) (ResourceType.t_trunkEnd - ResourceType.t_trunkStart);

    void Awake()
    {
        Instance = this;
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
        if ((int) type > (int)ResourceType.Max)
        {
            int a = 0;
        }
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
}
