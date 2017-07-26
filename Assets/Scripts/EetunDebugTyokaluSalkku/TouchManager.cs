using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchData
{
    public TouchData(Rect view, Action<Vector3> onTouchStart, Action<Vector3> onHold, Action onTouchEnded)
    {
        View = view;
        Active = false;
        FingerId = -1000;
        OnTouchStart = onTouchStart;
        OnHold = onHold;
        OnTouchEnded = onTouchEnded;
    }

    public Rect View;
    public bool Active;
    public int FingerId;
    public Action<Vector3> OnTouchStart;
    public Action<Vector3> OnHold;
    public Action OnTouchEnded;
}

public class TouchManager : MonoBehaviour
{
    private List<GameObject> _gameObjects = new List<GameObject>(10);
    private Sprite _sprite;
    private List<TouchData> _touchCallback = new List<TouchData>();

    private GameObject borders;
    private GameObject borders2;

    public static TouchManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void RegisterTouchCallbacks(TouchData data)
    {
        _touchCallback.Add(data);
    }

    GameObject CreateCursorDebugSprite()
    {
        GameObject go = new GameObject("debug touch point");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        // sr.sortingLayerName = "TouchController";
        // go.SetActive(false));
        return go;
    }

    void OnDrawGizmos()
    {
    }

    void Start()
    {
        _sprite = Resources.Load<Sprite>("Circle");

        borders = new GameObject("debug");
        // var sr = borders.AddComponent<SpriteRenderer>();
        // sr.sprite = Resources.Load<Sprite>("Square");
// 
        borders2 = new GameObject("debug");
        // sr = borders2.AddComponent<SpriteRenderer>();
        // sr.sprite = Resources.Load<Sprite>("Square");

        for (int i = 0; i < 10; i++)
        {
            _gameObjects.Add(CreateCursorDebugSprite());
        }


    }

    void Update()
    {
        Touch[] touches = Input.touches;
        Rect ScreenView = new Rect();

        for (int i = 0; i < _touchCallback.Count; i++)
        {
            Rect HitBox = _touchCallback[i].View;
            Vector3 cameraPos = Camera.main.transform.position;
            ScreenView = new Rect(cameraPos.x + HitBox.x, cameraPos.y + HitBox.y, HitBox.width, HitBox.height);
            // _touchCallback[i].View = ScreenView;
            // borders.transform.position = new Vector3(ScreenView.x, ScreenView.y, 5f);
            // borders2.transform.position = new Vector3(ScreenView.xMax, ScreenView.yMax, 5f);
        }


        for (int i = 0; i < touches.Length; i++)
        {
            if (i > _gameObjects.Count - 1)
            {
                break;
            }
            var touch = touches[i];
            var go = _gameObjects[i];
            if (touch.phase == TouchPhase.Began)
            {
                go.SetActive(true);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // go.SetActive(false);
            }

            Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
            go.transform.position = position;

            for (int j = 0; j < _touchCallback.Count; j++)
            {
                var touchCallback = _touchCallback[j];

                if (ScreenView.Contains(position)) // on sisällä
                {
                    if (!touchCallback.Active) // alkaa
                    {
                        touchCallback.FingerId = touch.fingerId;
                        touchCallback.Active = true;
                        touchCallback.OnTouchStart(position);
                    }
                    else
                    {
                        if (touchCallback.FingerId == touch.fingerId) // onko oikea sormi
                        {
                            if (touch.phase == TouchPhase.Ended)
                            {
                                touchCallback.OnTouchEnded();
                                touchCallback.Active = false;
                            }
                            else
                            {
                                touchCallback.OnHold(position);
                            }
                        }
                    }
                }
                else
                {
                    // COPY
                    if (!touchCallback.Active) // alkaa
                    {
                        continue;
                    }
                    else
                    {
                        if (touchCallback.FingerId == touch.fingerId) // onko oikea sormi
                        {
                            if (touch.phase == TouchPhase.Ended)
                            {
                                touchCallback.OnTouchEnded();
                                touchCallback.Active = false;
                            }
                            else
                            {
                                touchCallback.OnHold(position);
                            }
                        }
                    }
                }
            }
        }
    }
}
