using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class CraftingUiController : MonoBehaviour
{
    // Lapsena 9 Imagea
    private Image[] _hudImages = new Image[9];
    private Text[] _hudItemCounts = new Text[9];

    private GameObject[] _hudGameObjects = new GameObject[9];

    private Cost[] _cost = new Cost[9];
    private bool _inventoryActive = true;

    public Transform Images;
    public Transform Numbers;

    public IngredientType[] IngredientTyperOrders;

    // 1 -> toiseens
    private Dictionary<IngredientType, int> IndexMappings = new Dictionary<IngredientType, int>((int)IngredientType.Max);


    void Start()
    {
        _hudImages     = Images.GetComponentsInChildren<Image>();
        _hudItemCounts = Numbers.GetComponentsInChildren<Text>();

        for (int i = 0; i < _hudImages.Length; i++)
        {
            _hudGameObjects[i] = _hudImages[i].gameObject;
        }

        for (int i = 0; i < IngredientTyperOrders.Length; i++)
        {
            IndexMappings[IngredientTyperOrders[i]] = i;
        }

        CraftingManager.Instance.OnResourceCountChanged += CheckResourceNumbers;

        SetAllCounts();
    }

    void SetAllCounts()
    {
        for (int i = 0; i < (int)IngredientType.Max; i++)
        {
            int index = IndexMappings[(IngredientType)i];
            _hudItemCounts[index].text = CraftingManager.Instance.GetInventoryCount((IngredientType)i).ToString();
        }
    }

    void CheckResourceNumbers()
    {
        SetAllCounts();
    }

    //void SetCounts()
    //{
        
    //}

    //void SetImages(Sprite[] sprites)
    //{
        
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // set active
        {
            _inventoryActive = !_inventoryActive;
            {
                Images.gameObject.SetActive(_inventoryActive);
                Numbers.gameObject.SetActive(_inventoryActive);
            }
        }
    }
}

