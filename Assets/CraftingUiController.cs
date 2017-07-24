using System.Collections;
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
        InCombat,
        Max,
    }

    // Lapsena 9 Imagea
    private Image[] _hudImages = new Image[9];
    private Text[] _hudItemCounts = new Text[9];
    private GameObject[] _hudGameObjects = new GameObject[9];
    private bool[] _buttonVibrating = new bool[9];


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
    public Sprite CombatSprite;
    public Transform ResourceEndLocation;

    private Dictionary<ButtonState, Sprite[]> buttonStateSprites;
    private readonly int maxButtons = 9;

    private ButtonState current;

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

        buttonStateSprites = new Dictionary<ButtonState, Sprite[]>(5);
        buttonStateSprites[ButtonState.Default] = DefaultSprites;
        buttonStateSprites[ButtonState.Craft] = CraftSprites;
        buttonStateSprites[ButtonState.Light] = LightSprites;
        buttonStateSprites[ButtonState.OutOf] = OutOfSprites;

        Sprite[] arr = new Sprite[9];
        for (int i = 0; i < 9; i++)
        {
            arr[i] = CombatSprite;
        }
        buttonStateSprites[ButtonState.InCombat] = arr;

        current = ButtonState.InCombat;
        SetAllActiveState(true);

        BaseManager.Instance.RegisterOnBaseEnter(OnBaseEnter);
        BaseManager.Instance.RegisterOnBaseExit(OnBaseExit);

        // SetAllCounts();

        SetAllButtonsImages(ButtonState.InCombat);
    }

    void OnBaseEnter()
    {
        SetAllButtonsImages(ButtonState.Craft);
        SetAllActiveState(true);
        current = ButtonState.Craft;

        SetAllCounts();
    }

    void OnBaseExit()
    {
        SetAllButtonsImages(ButtonState.Default);
        current = ButtonState.Default;
        SetAllButtonsImages(ButtonState.InCombat);
    }

    void SetAllActiveState(bool state)
    {
        Images.gameObject.SetActive(state);
        Numbers.gameObject.SetActive(state);
    }

    public void SetAllButtonsImages(ButtonState state)
    {
        Sprite[] sprites = buttonStateSprites[state];
        for (int i = 0; i < maxButtons; i++)
        {

            _hudImages[i].sprite = sprites[i];
        }
    }

    public void SetButtonImage(ButtonState state, int x, int y)
    {
        _hudImages[y * 3 + x].sprite = buttonStateSprites[state][y * 3 + x];
    }

    public void SetButtonImageInvertedY(ButtonState state, int x, int y)
    {
        int yy = 2 - y;
        _hudImages[yy * 3 + x].sprite = buttonStateSprites[state][yy * 3 + x];
    }

    public void SetButtonColorInvertedY(Color color, int x, int y)
    {
        int yy = 2 - y;
        _hudImages[yy * 3 + x].sprite = buttonStateSprites[ButtonState.InCombat][yy * 3 + x];
        _hudImages[yy * 3 + x].color = color;
    }

    public void ResetAllColors()
    {
        foreach (var image in _hudImages)
        {
            image.color = Color.white;
        }
    }

    public void SetAllCounts()
    {
        for (int i = 0; i < (int)IngredientType.Max; i++)
        {
            int index = IndexMappings[(IngredientType)i];
            int count = CraftingManager.Instance.GetInventoryCount((IngredientType)i);
            _hudItemCounts[index].text = count.ToString();

            int x = index % 3;
            int y = (index - x) / 3;
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

    public void HideNumbers()
    {
        Numbers.gameObject.SetActive(false);
    }

    public void Vibrate(int x, int y)
    {
        if (!_buttonVibrating[y * 3 + x])
        {
            Vector3 startPosition = transform.position;
            {
                StartCoroutine(Vibrate(1f, 0.25f, x, y));
            }
        }
    }

    IEnumerator Vibrate(float seconds, float fibrationRange, int x, int y)  // TODO: MIETI iteraatiot oikein 
    {
        _buttonVibrating[y * 3 + x] = true;
        // transform.position = go.transform.position;
        Image go = _hudImages[y * 3 + x];

        int iterations = 30;
        float waitTime = seconds / (float)iterations;
        Vector3 startingPosition = go.transform.position;

        for (int i = 0; i < iterations; i++)
        {
            if (i % 2 == 0)
            {
                go.transform.Translate(Random.Range(-fibrationRange, fibrationRange), Random.Range(-fibrationRange, fibrationRange), 0f);
            }
            else
            {
                go.transform.Translate(Random.Range(-fibrationRange, fibrationRange), Random.Range(-fibrationRange, fibrationRange), 0f);
            }
            yield return new WaitForSeconds(waitTime);
        }
        go.transform.position = startingPosition;

        _buttonVibrating[y * 3 + x] = false;
    }

}


