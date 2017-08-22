#define DEV
using System;
using System.Collections;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    private static readonly int maxRuneIndices = 9;
    public Vec2[] runeIndices = new Vec2[maxRuneIndices];
    private int[] _touchCounts = new int[9];
    private int _currentCount = 0;

    public int amountOfSpheres = 3;
    public float offset = 10f;
    public float Radius = 5f;
    public float LineStartWidth = 0.5f;
    public float MaxWidht = 2f;

    private float _timer = 0f;
    public float lineResetTime = 2000f;

    private LineRenderer lineRenderer;

    private Vector3[] positions;
    public int _nextIndex = 0;
    private GameObject[] _colliders = new GameObject[9]; // yhdeksän kosketus kohtaa
    private bool _touching = false;

    public GameObject touchCollider;
    public GameObject Character;

    public float offsetX;
    public float offsetY;

    // TODO: ^^^ CLEANUP ^^^
    public RuneHolder RuneHolder;
    public RuneHolder CraftingManagerHolder;
    public Color ButtonColor;
    private GameObject knob;

    private Vector3 lastPosition;

    private CraftingUiController _craftingUiController;
    private combat _player;

    public enum Mode
    {
        Crafting,
        RuneCasting
    }

    public Mode ControllerMode = Mode.Crafting;
    private CurvedLineRendererController LineController;
    private bool _canSendIndices = false;

    //private delegate Action SendIndicesCallback;
    // public Delegate RunePillarsCallBack;
    // public event OnCraftingResourceChanged OnResourceCountChanged;

    void SendIndices()
    {
#if !DEV
        if (ControllerMode == Mode.RuneCasting && _canSendIndices)
        {
            RuneHolder.SendIndices(BoolArrayFromIndices(runeIndices), _touchCounts);
            _player.GetComponent<BaseChecker>().SendIndices(BoolArrayFromIndices(runeIndices), _touchCounts);
        }
        else if (ControllerMode == Mode.Crafting && _canSendIndices)
        {
            CraftingManagerHolder.SendIndices(BoolArrayFromIndices(runeIndices), _touchCounts);
        }
#else
        if (ControllerMode == Mode.RuneCasting && _canSendIndices)
        {
            RuneHolder.SendIndices(GenerateInBetweenPositions(runeIndices, _nextIndex), _nextIndex);
            _player.GetComponent<BaseChecker>().SendIndices(GenerateInBetweenPositions(runeIndices, _nextIndex), _nextIndex);
        }
        else if (ControllerMode == Mode.Crafting && _canSendIndices)
        {
            CraftingManagerHolder.SendIndices(GenerateInBetweenPositions(runeIndices, _nextIndex), _nextIndex);
        }
#endif

        // 
        _canSendIndices = false;
    }

    public static Vec2[] GenerateInBetweenPositions(Vec2[] indices, int size)
    {
        Vec2[] value;
        value = size > 0 ? new Vec2[size - 1] : new Vec2[0];


        for (int i = 0; i < size - 1; i++)
        {
            value[i] = indices[i] + indices[i + 1];
        }

        //Array.Sort(indices, (Vec2 v1, Vec2 v2) =>
        //{
        //    if (v1.X < v2.X)
        //    {
        //        return 1;
        //    }
        //    else if (v1.Y < v2.Y)
        //    {
        //        return 1;
        //    }
        //    return -1;
        //});

        string s = "";
        foreach (var v in value)
        {
            s += "(" + v.X + ", " + v.Y + ") ";
        }
        print(s);

        return value;
    }


    void DrawToMouse(Vector2 mouse)
    {
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
                Gizmos.DrawWireSphere(new Vector3(transform.position.x + x * offsetX, transform.position.y + y * offsetY, 0), Radius);
            }
        }
    }
    // Use this for initialization
    private float sceenXHack;
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // TODO: remove
        sceenXHack = Screen.width / 10f;
        // screenX = Screen.width / 2f;
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
                coll.transform.position = new Vector3(x * offsetX /*+ transform.position.x*/, y * offsetY /*+ transform.position.y*/, 0);
                coll.radius = Radius;
                _colliders[ii] = go;
                coll.isTrigger = true;

                // var sr = go.AddComponent<SpriteRenderer>();
                // sr.sprite = Resources.Load<Sprite>("Circle");

                // go.layer = SortingLayer.GetLayerValueFromName("TouchController");
                // TouchScreenPoint newPoint = new TouchScreenPoint(x, y);

                go.AddComponent(typeof(TouchScreenPoint));
                var point = go.GetComponent<TouchScreenPoint>();
                point.x = x;
                point.y = y;

                ii++;
                // _colliders[size].transform.position = new Vector3(start.x + x * offset, start.y + y * offset, 0);
                // _colliders[size].radius = Radius;
            }
        }

        lineRenderer.sortingOrder = 1;
        lineRenderer.sortingLayerName = "Foreground";

        for (int i = 0; i < maxRuneIndices; i++)
        {
            runeIndices[i] = new Vec2(0, 0);
        }
        _nextIndex = 0;
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

        _player = GameObject.FindWithTag("Player").GetComponent<combat>();

        lastPosition = transform.position;

        StartCoroutine(FixEnumeration());
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
    private bool _init = true;
    private Vector2 startPosition = new Vector2(0f, 0f);

    public static Vector2 endPosition = new Vector2(0f, 0f);
    public static Vector2 AttackDir = new Vector2(0f, 0f);

    void Update()
    {
#if !UNITY_EDITOR           // todo: bugittaako enää XDDDDDDDDD
        if (_init == false) // todo: miksi bugittaa androidilla
        {
            // SetTouchContollerCenters(_craftingUiController.GetPos());
            _init = true;
        }

        Touch[] touches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (touches[i].position.x > Screen.width / 2f + sceenXHack * 2f)
            {
                if (FingerId == -1000)
                {
                    FingerId = touches[i].fingerId;
                    startPosition = touches[i].position;
                }
            }

            if (touches[i].fingerId == FingerId) // oikea sormi liikkellä 
            {
                var mousePos = Camera.main.ScreenToWorldPoint(touches[i].position);
                UpdateTouchController(mousePos);

                if (touches[i].phase == TouchPhase.Ended)
                {
                    OnTouchEnded();

                    FingerId    = -1000;
                    var delta   = touches[i].position - startPosition;
                    endPosition = touches[i].position;
                    AttackDir   = delta;

                    _player.attackBoolean(delta,endPosition);

                    if (_player.GetComponent<combat>().hp > 0)
	                {
		                _player.GetComponent<AnimatorScript>().Attack();
                        _player.DoDamage(); 
	                }

                }
            }
        }
#else
        if (Input.GetMouseButton(0) /*|| Input.GetTouch(0).*/ )
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateTouchController(mousePos);
        }
        else if (_canSendIndices)
        {
            OnTouchEnded();

            var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position;
            AttackDir = delta;
            _player.attackBoolean(delta, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (_player.GetComponent<combat>().hp > 0)
            {
                _player.GetComponent<AnimatorScript>().Attack();
                _player.DoDamage();
            }

            // LineController.MovePoints(lastPosition - transform.position);
        }
        lastPosition = transform.position;
#endif

        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector2[] pos = _craftingUiController.GetPos();
            foreach (var p in pos)
            {
                Debug.LogErrorFormat("{0}", p);
            }
        }
    }

    IEnumerator FixEnumeration()
    {
        while (true)
        {
            if (_craftingUiController.Initted)
            {
                for (int i = 0; i < 10; i++)
                {
                    SetTouchContollerCenters(_craftingUiController.GetPos());
                    yield return new WaitForSeconds(0.2f);
                }
                break;
            }
            yield return null;
        }

        print("done");
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
        _nextIndex = 0;

        touchCollider.GetComponent<Collider2D>().enabled = false;
        _touching = false;
        _timer -= 200;
        ResetColliders();

        SetLineRendererCount(0);

        if (Mode.Crafting == ControllerMode)
        {
            _craftingUiController.SetAllCounts(true);
        }
        else
        {
            _craftingUiController.HideNumbers();
            _craftingUiController.SetAllButtonsImages(CraftingUiController.ButtonState.InCombat);
            _craftingUiController.ResetToTransparent();
        }
        LineController.ResetPoints();
        LineController.HideLines();

        for (int i = 0; i < _touchCounts.Length; i++)
        {
            _touchCounts[i] = 0;
        }

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

    public void AddTouchCount(int x, int y)
    {
        _touchCounts[y * 3 + x]++;
    }


    // private Vec2[] _touchPoints = new Vec2[maxRuneIndices];
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
#if true
            Vector3 point = realTransform - transform.position;
            LineController.SetPoint(new Vector3(transform.position.x + point.x, transform.position.y + point.y, 4f));

            if (_nextIndex < maxRuneIndices)
            {
                runeIndices[_nextIndex] = new Vec2(x, y);
                AddTouchCount(x, y);
                _nextIndex++;
            }
#else
            // visuaali
            Vector3 point = realTransform - transform.position;
            LineController.SetPoint(new Vector3(transform.position.x + point.x, transform.position.y + point.y, 4f));

            if (size < maxRuneIndices)
            {
                if (size != 0)
                {
                    runeIndices[size] = new Vec2(x, y);
                    AddTouchCount(x, y);
                    size++;
                }
                else
                {

                }
            }
#endif
        }
        _timer = Time.time + lineResetTime;
    }

    public bool[] BoolArrayFromIndices(Vec2[] indices)
    {
        bool[] value = new bool[9];
        for (int i = 0; i < _nextIndex; i++)
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

    public void SetTouchContollerCenters(Vector2[] positions)
    {
        // käännetään y ylösalaisin
        for (int y = 0, i = 0; y < 3; y++)
        {
            int yy = 2 - y;
            for (int x = 0; x < 3; x++, i++)
            {
                _colliders[yy * 3 + x].transform.position = positions[i];
            }
        }
    }
}
