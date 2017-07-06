using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public RuneHolder RuneHolder;

    private static readonly int maxRuneIndices = 9;
    private Vec2[] runeIndices = new Vec2[maxRuneIndices];
    public Sprite BulletSprite;

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
    private int index = 0;
    private GameObject[] _colliders = new GameObject[9]; // yhdeksän kosketus kohtaa
    private bool _touching = false;

    public GameObject touchCollider;

    public GameObject TrafficLights;
    public GameObject Character;

    public Material lineMaterial;

    public float screenX = 600f;


    // TODO: ^^^ CLEANUP ^^^
    private int SlashLineIndex = 0;
    public static readonly  int MaxLineIndices = 15;
    private Vector3[] SlashLineIndices = new Vector3[MaxLineIndices];
    private int LineIndex = 0;
    private Vector3[] LineIndices = new Vector3[100];

    public float LineIndexDistance = 0.10f;


    void AddSlashLineIndex(Vector3 position)
    {
        if (SlashLineIndex < MaxLineIndices)
        {
            SlashLineIndices[SlashLineIndex] = position;
            lineRenderer.positionCount = SlashLineIndex + 1;
            lineRenderer.SetPosition(SlashLineIndex, SlashLineIndices[SlashLineIndex]);

            SlashLineIndex++;
        }
    }

    void ResetSlashLineIndices()
    {
        SlashLineIndex = 0;
    }

    void DrawToMouse(Vector2 mouse)
    {
        lineRenderer.positionCount = SlashLineIndex + 1;
        lineRenderer.SetPosition(SlashLineIndex, mouse);
    }


    //void AddLineIndices(Vector2 position)
    //{
    //    if (LineIndex != 0)
    //    {
    //        if (Vector2.Distance(position, LineIndices[LineIndex - 1]) > LineIndexDistance)
    //        {
    //            if (LineIndex < LineIndices.Length)
    //            {
    //                LineIndices[LineIndex] = new Vector3(position.x, position.y, 2f);
    //                LineIndex++;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        LineIndices[LineIndex] = new Vector3(position.x, position.y, 2f);
    //        LineIndex++;
    //    }

    //    lineRenderer.numPositions = LineIndex;
    //    lineRenderer.SetPosition(LineIndex - 1, LineIndices[LineIndex - 1]);
    //}

    //void ResetLineRenderer()
    //{
    //    lineRenderer.numPositions = 0;
    //}

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
        screenX = Screen.width / 2;
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
                var go = Instantiate(new GameObject(), parent.transform);
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
        lineRenderer.material = lineMaterial;
        lineRenderer.sortingOrder = 1;
        lineRenderer.sortingLayerName = "Foreground";

        for (int i = 0; i < maxRuneIndices; i++)
        {
            runeIndices[i] = new Vec2(0, 0);
        }
        index = 0;
    }
    private int FingerId = -1000;

    void Update()
    {
#if false               // MOBILERLKj
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (myTouches[i].position.x > screenX)
            {
                if (FingerId == -1000)
                {
                    FingerId = myTouches[i].fingerId;
                }

                if (myTouches[i].fingerId == FingerId) // oikea sormi liikkellä 
                {
                    var mousePos = Camera.main.ScreenToWorldPoint(myTouches[i].position);
                    mousePos.z = 2;
                    touchCollider.GetComponent<Collider2D>().enabled = true;
                    touchCollider.transform.position = mousePos;
                    _touching = true;

                    if (myTouches[i].phase == TouchPhase.Ended) // loppu
                    {
                        FingerId = -1000;
                        touchCollider.transform.position = myTouches[i].position;
                        touchCollider.GetComponent<Collider2D>().enabled = false;

                        // make vector of Attack Direction
                        Vector2 touchDeltaVector = myTouches[i].deltaPosition;

                        var bulletGo = Instantiate(new GameObject());
                        bulletGo.transform.position = Character.transform.position;
                        var bullet = bulletGo.AddComponent<Bullet>();
                        bullet.velocity = touchDeltaVector.normalized;
                        bullet.Speed = bulletSpeed;

                        var renderer = bulletGo.AddComponent<SpriteRenderer>();
                        renderer.sprite = BulletSprite;
                        renderer.sortingLayerName = "Player";

                        index = 0;
                        ResetColliders();
                        touchCollider.GetComponent<Collider2D>().enabled = false;
                        _touching = false;
                    }
                }
            }
        }
#endif
        //if (myTouches.Length == 0)
        //{
        //    TrafficLights.GetComponent<SpriteRenderer>().material.color = Color.red;
        //}
        //else if (myTouches.Length == 1)
        //{
        //    TrafficLights.GetComponent<SpriteRenderer>().material.color = Color.blue;
        //}
        //else if (myTouches.Length >= 2)
        //{
        //    TrafficLights.GetComponent<SpriteRenderer>().material.color = Color.green;
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Vector2 touchDeltaVector = new Vector2(1, 1);

        //    var bulletGo = Instantiate(new GameObject());
        //    bulletGo.transform.position = Character.transform.position;
        //    var bullet = bulletGo.AddComponent<Bullet>();
        //    bullet.velocity = touchDeltaVector.normalized;
        //    var renderer = bulletGo.AddComponent<SpriteRenderer>();
        //    renderer.sprite = BulletSprite;
        //}

        //Vector2 movement = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical"));
        //if (movement.x != 0 || movement.y != 0)
        //{
        //    Character.transform.Translate(movement * 3 * Time.deltaTime);
        //    Activate(movement * 3 * Time.deltaTime);
        //}

        if (Input.GetMouseButton(0) /*|| Input.GetTouch(0).*/ )
        {

            //if (LineFadeEffectRunning)
            //StopCoroutine(LineFadeEffect());
            //lineRenderer.widthMultiplier = LineStartWidth;
            //lineRenderer.positionCount = 0;
            //lineRenderer.numPositions = 0;
            //index = 0;
            //LineFadeEffectRunning = false;
            //lineActive = false;

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 2;

            touchCollider.GetComponent<Collider2D>().enabled = true;
            touchCollider.transform.position = mousePos;

            _touching = true;
            DrawToMouse((mousePos));
        }
        else
        {
            RuneHolder.SendIndices(runeIndices, index);
            index = 0;

            touchCollider.GetComponent<Collider2D>().enabled = false;
            _touching = false;
            _timer -= 200;
            ResetColliders();


            // sormi poesa
            ResetSlashLineIndices();
        }

        // Reset LineRenderer
        if (_timer < Time.time)
        {
            index = 0;
            LineIndex = 0;
            ResetSlashLineIndices();
            ResetColliders();
            ResetSlashLineIndices();
        }
    }

    // private bool lineActive = false;
    // private bool LineFadeEffectRunning = false;
    // public float TotalLineFadeEffectTime = 0.4f;
    // IEnumerator LineFadeEffect()                        // TODO: Hianompaa effectiä
    // {
    //     // MVP: Kommunikaatio Runejen laukaisun kannsa

    //    LineFadeEffectRunning = true;
    //    Color color = new Color(170, 170, 170, 120);
    //    lineRenderer.material.color = color;
    //    ResetColliders();

    //    // larger line every frame? 
    //    for (float i = 0; i < 30; i++)
    //    {
    //        lineRenderer.widthMultiplier = Mathf.Lerp(LineStartWidth, MaxWidht, (float)i / 30);
    //        yield return new WaitForSeconds(TotalLineFadeEffectTime / 30);
    //    }

    //    lineRenderer.widthMultiplier = LineStartWidth;

    //    lineRenderer.numPositions = 0;

    //    index = 0;
    //    LineFadeEffectRunning = false;
    //    lineActive = false;
    //}


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

        if (_touching)
        {
            AddSlashLineIndex(new Vector3(transform.position.x + x * offset, transform.position.y + y * offset, 4f));

            if (index < maxRuneIndices)
            {
                runeIndices[index] = new Vec2(x, y);
                positions[index] = new Vector3(transform.position.x + x * offset, transform.position.y + y * offset, 4f);
                index++;
            }
        }
        _timer = Time.time + lineResetTime;

    }
}
