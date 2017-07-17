using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUiController : MonoBehaviour
{
    public enum ButtonState
    {
        Default,
        Craft,
        Light,
        OutOf,
        Max,
    }

    // Lapsena 9 Imagea
    private Image[] _hudImages = new Image[9];
    private Text[] _hudItemCounts = new Text[9];

    private GameObject[] _hudGameObjects = new GameObject[9];

    // private Cost[] _cost = new Cost[9];
    // private bool _inventoryActive = true;

    public Transform Images;
    public Transform Numbers;

    public IngredientType[] IngredientTyperOrders;

    // 1 -> toiseens
    private Dictionary<IngredientType, int> IndexMappings = new Dictionary<IngredientType, int>((int)IngredientType.Max);


    public Sprite[] DefaultSprites;
    public Sprite[] CraftSprites;
    public Sprite[] LightSprites;
    public Sprite[] OutOfSprites;
    public Transform ResourceEndLocation;

    private Dictionary<ButtonState, Sprite[]> buttonStateSprites;
    private readonly int maxButtons = 9;

    void Start()
    {
        _hudImages = Images.GetComponentsInChildren<Image>();
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
        // CraftingManager.Instance.SetResourcePickupEndLocation(Camera.main.ScreenToWorldPoint(ResourceEndLocation.position));



        buttonStateSprites = new Dictionary<ButtonState, Sprite[]>(4);
        buttonStateSprites[ButtonState.Default] = DefaultSprites;
        buttonStateSprites[ButtonState.Craft] = CraftSprites;
        buttonStateSprites[ButtonState.Light] = LightSprites;
        buttonStateSprites[ButtonState.OutOf] = OutOfSprites;

        SetAllButtonsImages(ButtonState.Default);
        SetAllActiveState(false);

        BaseManager.Instance.RegisterOnBaseEnter(OnBaseEnter);
        BaseManager.Instance.RegisterOnBaseExit(OnBaseExit);

        SetAllCounts();
    }

    void OnBaseEnter()
    {
        SetAllButtonsImages(ButtonState.Craft);
        SetAllActiveState(true);

        SetAllCounts();
    }

    void OnBaseExit()
    {
        SetAllButtonsImages(ButtonState.Default);
        SetAllActiveState(false);
    }

    void SetAllActiveState(bool state)
    {
        Images.gameObject.SetActive(state);
        Numbers.gameObject.SetActive(state);
    }


    void SetAllButtonsImages(ButtonState state)
    {
        Sprite[] sprites = buttonStateSprites[state];
        for (int i = 0; i < maxButtons; i++)
        {
            _hudImages[i].sprite = sprites[i];
        }
    }

    void SetButtonImage(ButtonState state, int x, int y)
    {
        _hudImages[y * 3 + x].sprite = buttonStateSprites[state][y * 3 + x];
    }

    void SetAllCounts()
    {
        for (int i = 0; i < (int)IngredientType.Max; i++)
        {
            int index = IndexMappings[(IngredientType)i];
            int count = CraftingManager.Instance.GetInventoryCount((IngredientType)i);
            _hudItemCounts[index].text = count.ToString();

            int x = i % 3;
            int y = (i - x) / 3;
            SetButtonImage(count <= 0 ? ButtonState.OutOf : ButtonState.Craft, x, y);
        }
    }

    void CheckResourceNumbers()
    {
        SetAllCounts();
    }

    private int count = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // set active
        {
            //_inventoryActive = !_inventoryActive;
            //{
            //    Images.gameObject.SetActive(_inventoryActive);
            //    Numbers.gameObject.SetActive(_inventoryActive);
            //}

            SetAllButtonsImages((ButtonState)count++);
            if (count == (int)ButtonState.Max)
            {
                count = 0;
            }
        }
    }
}

