using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Initoidaan Awakessa")]
    public static ResourceManager Instance = null;
    private string[] resourceTypeLookupTable = new string[(int)ResourceType.Max];

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
    }

    public string GetResourceTypeName(ResourceType type)
    {
        return resourceTypeLookupTable[(int) type];
    }
}
