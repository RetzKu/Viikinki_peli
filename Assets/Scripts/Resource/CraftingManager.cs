using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    // ota playerriin reffi, jossain vaiheessa niin saadaan itemssit
    public GameObject Player;

    public static CraftingManager Instance = null;

    public delegate void OnCraftingResourceChanged();
    public event OnCraftingResourceChanged OnResourceCountChanged;

    private Dictionary<IngredientType, int> CraftingInventory = new Dictionary<IngredientType, int>((int)IngredientType.Max);

    // public Dictionary<IngredientType, Sprite> sprite = new Dictionary<IngredientType, Sprite>();
    // resuja actually 8 kpl atm

    public float DragTimerEffect = 3.5f;
    public float MaxRotation = 3.5f;
    public float MinRotation = 1.5f;
    public RectTransform InventoryPosition;

    // GUI 
    // Ota viestit vastaan rune castaukselta
    // Lataa spritet awakessa niin saadaan ne GUILLE
    private Vector3 _resourcePickupEndPosition = new Vector3(0f, 0f, 0f);


    public void SetResourcePickupEndLocation(Vector3 position)
    {
        _resourcePickupEndPosition = position;
    }


    void OnDrawGizmos()
    {
        if (InventoryPosition)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(InventoryPosition.position, 10f);
        }
    }

    void Awake()
    {
        CraftingManager.Instance = this;
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        OnResourceCountChanged += () => {  /* ääni */ };
    }

    // RuneHolder samalle gameobjectille, jolle CraftiReseptit
    // Resepti Itsessään laukaisee  TryCraftin();
    public bool TryToCraftItem(List<Cost> materials, GameObject prefab)
    {
        bool success = true;

        foreach (Cost material in materials)
        {
            if (GetInventoryCount(material.Type) - material.Count < 0)
            {
                success = false;
                break;
            }
        }

        if (success)
        {
            foreach (Cost cost in materials)
            {
                // huom vähentää
                AddToInventory(cost.Type, -cost.Count);
            }

            // TODO: Minne instantioin craftatun itemin
            var go = Instantiate(prefab);
            go.tag = "Dropped";
            go.transform.position = Player.transform.position;
            var faller = go.AddComponent<ObjectFaller>();
            faller.StartFreeFall(2.4f);

            // Update Gui
            if (OnResourceCountChanged != null)
                OnResourceCountChanged();
        }

        return success;
    }

    Cost[] GetAllResourcesCounts()
    {
        Cost[] value = new Cost[(int)IngredientType.Max];
        for (int i = 0; i < (int)IngredientType.Max; i++)
        {
            value[i] = new Cost((IngredientType)i, GetInventoryCount((IngredientType)i));
        }
        return value;
    }

    void AddToInventory(IngredientType type, int count = 1)
    {
        // print("added: " + type.ToString());
        CraftingInventory[type] = GetInventoryCount(type) + count;

        if (OnResourceCountChanged != null)
            OnResourceCountChanged();
    }

    public int GetInventoryCount(IngredientType type)
    {
        int count = 0;
        CraftingInventory.TryGetValue(type, out count);
        return count;
    }

    public void AddToInventory(GameObject go)
    {
        StartCoroutine(DragToInventoryEffect(go));
    }

    private IEnumerator DragToInventoryEffect(GameObject go)
    {
        if (InventoryPosition)
        {
            int iters = 50;
            Vector3 start = go.transform.position;

            float dir = Random.Range(0, 2) == 0 ? -1 : 1;
            float rotation = dir * Random.Range(MinRotation, MaxRotation);

            for (int i = 0; i < iters; i++)
            {
                go.transform.position = Vector3.Lerp(start, Camera.main.ScreenToWorldPoint(InventoryPosition.position), i / (float)iters);
                go.transform.Rotate(0f, 0f, rotation);
                yield return null;
            }
        }

        var ingredient = go.GetComponent<Ingredient>();
        AddToInventory(ingredient.Type);

        Destroy(go); // Lopussa himmennystä
    }

    public GridPositions[] positions;
    public Vec2 GetCraftingIndexes(IngredientType type)
    {
        foreach (var gridGridPosition in positions)
        {
            if (gridGridPosition.type == type)
            {
                return gridGridPosition.index;
            }
        }

        Debug.LogWarning("Warning type: " + type + " not found in CraftingManagerSettings");
        return new Vec2(-1, -1);
    }

    [System.Serializable]
    public class GridPositions
    {
        public Vec2 index;
        public IngredientType type;
    }
}

