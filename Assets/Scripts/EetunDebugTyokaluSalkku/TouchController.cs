using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Circle : MonoBehaviour
{
}


public class TouchController : MonoBehaviour
{
    public RuneHolder RuneHolder;
    private Vec2[] runeIndices = new Vec2[9];
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
        // start.x = 0f;
        // start.y = 0f;

        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[10];
        lineRenderer.SetPositions(positions);

        GameObject parent = new GameObject("colliders");
        parent.transform.position = transform.position;
        parent.transform.parent = this.transform;

        int index = 0;
        for (int y = 0; y < amountOfSpheres; y++)
        {
            for (int x = 0; x < amountOfSpheres; x++)
            {
                var go = Instantiate(new GameObject(), parent.transform);
                var coll = go.AddComponent<CircleCollider2D>();
                coll.transform.position = new Vector3(x * offset + transform.position.x, y * offset + transform.position.y, 0);
                coll.radius = Radius;
                _colliders[index] = go;
                coll.isTrigger = true;

                // go.layer = SortingLayer.GetLayerValueFromName("TouchController");

                // TouchScreenPoint newPoint = new TouchScreenPoint(x, y);
                go.AddComponent(typeof(TouchScreenPoint));
                var point = go.GetComponent<TouchScreenPoint>();
                point.x = x;
                point.y = y;


                index++;
                // _colliders[index].transform.position = new Vector3(start.x + x * offset, start.y + y * offset, 0);
                // _colliders[index].radius = Radius;
            }
        }
        lineRenderer.material = lineMaterial;
        lineRenderer.sortingOrder = 1;
        lineRenderer.sortingLayerName = "Line";
    }


    private int FingerId = -1000;

    void Update()
    {
        // LineRenderer.SetPositions(positions);
        // draw spheres
        // print(mousePos);
        // positions[index] = new Vector3(mousePos.x, mousePos.y, 0);

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

        if (myTouches.Length == 0)
        {
            TrafficLights.GetComponent<SpriteRenderer>().material.color = Color.red;
        }
        else if (myTouches.Length == 1)
        {
            TrafficLights.GetComponent<SpriteRenderer>().material.color = Color.blue;
        }
        else if (myTouches.Length >= 2)
        {
            TrafficLights.GetComponent<SpriteRenderer>().material.color = Color.green;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 touchDeltaVector = new Vector2(1, 1);

            var bulletGo = Instantiate(new GameObject());
            bulletGo.transform.position = Character.transform.position;
            var bullet = bulletGo.AddComponent<Bullet>();
            bullet.velocity = touchDeltaVector.normalized;
            var renderer = bulletGo.AddComponent<SpriteRenderer>();
            renderer.sprite = BulletSprite;
        }

        //Vector2 movement = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical"));
        //if (movement.x != 0 || movement.y != 0)
        //{
        //    Character.transform.Translate(movement * 3 * Time.deltaTime);
        //    Activate(movement * 3 * Time.deltaTime);
        //}

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0) /*|| Input.GetTouch(0).*/ )
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 2;

            touchCollider.GetComponent<Collider2D>().enabled = true;
            touchCollider.transform.position = mousePos;

            _touching = true;
        }
        else
        {
            // index = 0;
            touchCollider.GetComponent<Collider2D>().enabled = false;
            _touching = false;
            // ResetColliders();
            _timer -= 200;
        }


        // Reset LineRenderer
        if (lineActive && _timer < Time.time)
        {
            // kick of effecte coroutine ?
            if (!LineFadeEffectRunning)
                StartCoroutine(LineFadeEffect());
        }
   }


    private bool lineActive = false;
    private bool LineFadeEffectRunning = false;
    public float TotalLineFadeEffectTime = 0.4f;
    IEnumerator LineFadeEffect()
    {
        // MVP: Kommunikaatio Runejen laukaisun kannsa
        RuneHolder.SendIndices(runeIndices, index);

        LineFadeEffectRunning = true;
        Color color = new Color(170, 170, 170, 120);
        lineRenderer.material.color = color;
        
        // larger line every frame? 
        for (float i = 0; i < 30; i++)
        {
            lineRenderer.widthMultiplier = Mathf.Lerp(LineStartWidth, MaxWidht, (float)i / 30);
            yield return new WaitForSeconds(TotalLineFadeEffectTime / 30);
        }

        ResetColliders();
        lineRenderer.widthMultiplier = LineStartWidth;
        //lineRenderer.positionCount = 0;

        lineRenderer.numPositions = 0;

        index = 0;
        LineFadeEffectRunning = false;
        lineActive = false;
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

    // make line
    private void Activate(Vector2 movement)
    {
        for (int i = 0; i < index; i++)
        {
            positions[i].x += movement.x;
            positions[i].y += movement.y;
            lineRenderer.SetPosition(i, positions[i]);
        }
        // if (index > 1)
        // {
        //for (int i = 0; i < index; i++)
        //{
        //    LineRenderer.SetPosition(index, positions[index]);
        //}
        // }
    }

    public void OnTouchDetected(int x, int y)
    {
        lineActive = true;
        // var go = GetFromArray(x, y);
        if (_touching && index < 9)
        {
            positions[index] = new Vector3(transform.position.x + x * offset, transform.position.y + y * offset, 4f);
            print("x: " + x + " y: " + y);
            runeIndices[index] = new Vec2(x, y);
            //LineRenderer.positionCount = index + 1;

            //LineRenderer.positionCount = index + 1;
            //LineRenderer.positionCount = index + 1;
            //LineRenderer.positionCount = index + 1;
            //LineRenderer.positionCount = index + 1;
            //lineRenderer.positionCount = index + 1;
            //lineRenderer.positionCount = index + 1;
            lineRenderer.numPositions = index + 1;

            lineRenderer.SetPosition(index, positions[index]);
            lineRenderer.sortingLayerName = "Foreground";
            index++;
        }

        // Add timer? 
        _timer = Time.time + lineResetTime;
    }
    // fix: LineRenderer.numPositions = index + 1;
}
