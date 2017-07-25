using UnityEngine;

public class TouchController : MonoBehaviour
{
    private static readonly int maxRuneIndices = 9;
    public Vec2[] runeIndices = new Vec2[maxRuneIndices];

    public int amountOfSpheres = 3;
    public float offset = 10f;
    public float Radius = 5f;
    public float bulletSpeed = 4f;

    public float LineStartWidth = 0.5f;
    public float MaxWidht = 2f;

    private float _timer = 0f;
    public float lineResetTime = 2000f;

    private LineRenderer lineRenderer;

    private Vector3[] positions;
    public int index = 0;
    private GameObject[] _colliders = new GameObject[9]; // yhdeksän kosketus kohtaa
    private bool _touching = false;

    public GameObject touchCollider;
    public GameObject Character;


    public float screenX = 600f;


    // TODO: ^^^ CLEANUP ^^^
    public RuneHolder RuneHolder;
    public RuneHolder CraftingManagerHolder;
    public Color ButtonColor;
    private GameObject knob;

    private Vector3 lastPosition;

    private CraftingUiController _craftingUiController;

    public enum Mode
    {
        Crafting,
        RuneCasting
    }

    public Mode ControllerMode = Mode.Crafting;
    private CurvedLineRendererController LineController;
    private bool _canSendIndices = false;

    void SendIndices()
    {
        if (ControllerMode == Mode.RuneCasting && _canSendIndices)
        {
            RuneHolder.SendIndices(BoolArrayFromIndices(runeIndices));
        }
        else if (ControllerMode == Mode.Crafting && _canSendIndices)
        {
            CraftingManagerHolder.SendIndices(BoolArrayFromIndices(runeIndices));
        }
        _canSendIndices = false;
    }

    void DrawToMouse(Vector2 mouse)
    {
        // lineRenderer.positionCount = SlashLineIndex + 1;  // <-- uudempi unity kuin 5.5.1f1
        // lineRenderer.positionCount = SlashLineIndex + 1;     // <-- unity 5.5.1f1
        // lineRenderer.SetPosition(SlashLineIndex, new Vector3(mouse.x, mouse.y, 4f));
        if (LineController.GetLinePointCount() == 1)
        {


            SetLineRendererCount(0);
            knob.transform.position = new Vector3(-100, -100, -100);
        }
        else
        {
            SetLineRendererCount(2);
            lineRenderer.SetPosition(0, LineController.GetLastPointPosition());
            lineRenderer.SetPosition(1, new Vector3(mouse.x, mouse.y, 4f));
            knob.transform.position = LineController.GetLastPointPosition();
        }
    }
    void OnDrawGizmos()
    {
        for (int y = 0; y < amountOfSpheres; y++)
        {
            for (int x = 0; x < amountOfSpheres; x++)
            {
                Gizmos.DrawWireSphere(new Vector3(transform.position.x + x * offset, transform.position.y + y * offset, 0), Radius);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        screenX = Screen.width / 2f;
        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[10];
        lineRenderer.SetPositions(positions);

        GameObject parent = new GameObject("colliders");
        parent.transform.position = transform.position;
        parent.transform.parent = this.transform;

        int ii = 0;

        for (int y = 0; y < amountOfSpheres; y++)
        {
            for (int x = 0; x < amountOfSpheres; x++)
            {
                var go = new GameObject();
                go.layer = LayerMask.NameToLayer("TouchController");
                go.transform.parent = transform;
                var coll = go.AddComponent<CircleCollider2D>();
                coll.transform.position = new Vector3(x * offset + transform.position.x, y * offset + transform.position.y, 0);
                coll.radius = Radius;
                _colliders[ii] = go;
                coll.isTrigger = true;

                // go.layer = SortingLayer.GetLayerValueFromName("TouchController");

                // TouchScreenPoint newPoint = new TouchScreenPoint(x, y);

                go.AddComponent(typeof(TouchScreenPoint));
                var point = go.GetComponent<TouchScreenPoint>();
                point.x = x;
                point.y = y;

                ii++;
                // _colliders[index].transform.position = new Vector3(start.x + x * offset, start.y + y * offset, 0);
                // _colliders[index].radius = Radius;
            }
        }

        lineRenderer.sortingOrder = 1;
        lineRenderer.sortingLayerName = "Foreground";

        for (int i = 0; i < maxRuneIndices; i++)
        {
            runeIndices[i] = new Vec2(0, 0);
        }
        index = 0;
        SetLineRendererCount(0);

        // Setup Crafting System
        CraftingManagerHolder = CraftingManager.Instance.GetComponent<RuneHolder>();

        BaseManager.Instance.RegisterOnBaseEnter(OnBaseEnter);
        BaseManager.Instance.RegisterOnBaseExit(OnBaseExit);

        ControllerMode = Mode.RuneCasting;

        // aka pelaaja
        RuneHolder = GameObject.FindWithTag("Player").GetComponent<RuneHolder>();

        var linecontroller = Instantiate(Resources.Load<GameObject>("Prefab/Ui/LineController"));
        LineController = linecontroller.GetComponent<CurvedLineRendererController>();

        if (LineController == null)
        {
            Debug.LogError("Please add CurvedLineRendererController to player/TouchControls/Controller object");
        }

        knob = Instantiate(Resources.Load<GameObject>("Prefab/Ui/LineRendererEndPoint"));

        if (knob == null)
        {
            Debug.LogWarning("Cannot load LineRendererEndPoint plz tell Eetu");
        }

        _craftingUiController = GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>();


        lastPosition = transform.position;
    }

    void OnBaseEnter()
    {
        ControllerMode = Mode.Crafting;
    }

    void OnBaseExit()
    {
        ControllerMode = Mode.RuneCasting;
    }

    void SetLineRendererCount(int i)
    {
        lineRenderer.numPositions = i;
    }

    private int FingerId = -1000;

    void Update()
    {
#if UNITY_ANDROID
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (myTouches[i].position.x > screenX)
            {
                // trackaa oikeaa sormea
                if (FingerId == -1000)
                {
                    FingerId = myTouches[i].fingerId;
                }


                if (myTouches[i].fingerId == FingerId) // oikea sormi liikkellä 
                {
                    var mousePos = Camera.main.ScreenToWorldPoint(myTouches[i].position);
                    UpdateTouchController(mousePos);

                    // mousePos.z = 2;
                    // touchCollider.GetComponent<Collider2D>().enabled = true;
                    // touchCollider.transform.position = mousePos;
                    // _touching = true;

                    if (myTouches[i].phase == TouchPhase.Ended) // loppu
                    {
                        OnTouchEnded();
                        FingerId = -1000;
                        // touchCollider.transform.position = myTouches[i].position;
                        // touchCollider.GetComponent<Collider2D>().enabled = false;
                        // 
                        //  make vector of Attack Direction
                        // Vector2 touchDeltaVector = myTouches[i].deltaPosition;
                        // 
                        // index = 0;
                        // ResetColliders();
                        // touchCollider.GetComponent<Collider2D>().enabled = false;
                        // _touching = false;
                    }
                }
            }
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) /*|| Input.GetTouch(0).*/ )
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateTouchController(mousePos);
        }
        else
        {
            OnTouchEnded();

            // LineController.MovePoints(lastPosition - transform.position);
        }
        lastPosition = transform.position;
#endif
    }

    private void UpdateTouchController(Vector3 mousePos)
    {
        _canSendIndices = true;
        mousePos.z = 2;

        touchCollider.GetComponent<Collider2D>().enabled = true;
        touchCollider.transform.position = mousePos;

        _touching = true;
        DrawToMouse((mousePos));

        LineController.transform.position = transform.position;
    }

    private void OnTouchEnded()
    {
        SendIndices();
        index = 0;

        touchCollider.GetComponent<Collider2D>().enabled = false;
        _touching = false;
        _timer -= 200;
        ResetColliders();

        SetLineRendererCount(0);

        if (Mode.Crafting == ControllerMode)
        {
            _craftingUiController.SetAllCounts();
        }
        else
        {
            _craftingUiController.HideNumbers();
            _craftingUiController.SetAllButtonsImages(CraftingUiController.ButtonState.InCombat);
        }
        _craftingUiController.ResetAllColors();
        LineController.ResetPoints();
        LineController.HideLines();

        _canSendIndices = false;
    }

    private GameObject GetFromArray(int x, int y)
    {
        return _colliders[y * amountOfSpheres + x];
    }

    private void ResetColliders()
    {
        foreach (var colliderGO in _colliders)
        {
            colliderGO.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    public void OnTouchDetected(int x, int y, Vector3 realTransform)
    {
        ResetColliders();
        if (ControllerMode == Mode.RuneCasting)
        {
            _craftingUiController.SetButtonColorInvertedY(ButtonColor, x, y);
        }
        else
        {
            _craftingUiController.SetButtonImageInvertedY(CraftingUiController.ButtonState.Light, x, y);
        }

        if (_touching)
        {
            LineController.SetPoint(new Vector3(transform.position.x + x * offset, transform.position.y + y * offset, 4f));
            if (index < maxRuneIndices)
            {
                runeIndices[index] = new Vec2(x, y);
                index++;
            }
        }
        _timer = Time.time + lineResetTime;
    }

    public bool[] BoolArrayFromIndices(Vec2[] indices)
    {
        bool[] value = new bool[9];
        for (int i = 0; i < index; i++)
        {
            int iii = GetBoolIndex(indices[i]);
            value[iii] = true;
        }
        return value;
    }

    public static readonly int[,] IndexLookUpTable = new int[,] // x, y
    {
        { 0, 1, 2 },
        { 3, 4, 5 },
        { 6, 7, 8 },
    };

    public static int GetBoolIndex(Vec2 v)
    {
        return IndexLookUpTable[v.Y, v.X];
    }
}
