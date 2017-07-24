// #define MOUSE

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

public enum ControllerType
{
    Touch,
    Mouse
}

public class CustomJoystick : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 endposition;
    private bool touching = false;
    private bool touchingLastFrame = false;

    public Vector2 offsets = new Vector2(0, 0);

    public Sprite JoystickSprite;

    public float maxLength;
    public ControllerType controlScheme = ControllerType.Mouse;
    public Rect HitBox;

    public Transform Player;

    private Vector3 lastPos;
    private Vector3 position;

    private bool FirstTouchSuccess = false;
#pragma warning disable 108,114
    SpriteRenderer renderer;
#pragma warning restore 108,114

    public Sprite Head;
    // public Sprite BaseSprite;

    private GameObject Base;

    private static readonly int NO_FINGER = -1000;
    private int fingerId = NO_FINGER;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPosition, (Vector3)startPosition + (Vector3)GetTouchVector().normalized);

        // Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 position = Camera.main.transform.position + (Vector3)HitBox.center;
        Gizmos.DrawWireCube(position, new Vector3(HitBox.width, HitBox.height));
    }

    void Start()
    {
        startPosition = new Vector2(0f, 0f);
        endposition = new Vector2(0f, 0f);
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = JoystickSprite;

        position = new Vector3(0f, 0f, 0f);
        renderer.sprite = Head;


        Base = Instantiate(Resources.Load<GameObject>("Prefab/Ui/JoystickBase"));

    }

    // TODO: Cleanup
    void Update()
    {
        if (controlScheme == ControllerType.Mouse)
        {
            MouseUpdate();
            return;
        }

        // Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        // transform.Translate(lastPos - transform.position);
        Vector3 movVec = Player.transform.position - position;
        startPosition = startPosition + (Vector2)movVec;
        endposition = endposition + (Vector2)movVec;

        position = Player.transform.position;
#if MOUSE
        touching = Input.GetMouseButton(0);
#else
        Touch[] touches = Input.touches;

        if (touches.Length != 0)
            touching = true;
#endif
        Vector3 currentPotition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // sormi
        Vector3 cameraPos = Camera.main.transform.position;
        Rect ScreenView = new Rect(cameraPos.x + HitBox.x, cameraPos.y + HitBox.y, HitBox.width, HitBox.height);

        Base.transform.position = startPosition;
        Base.transform.position = Vector3.zero;
#if MOUSE
        if (Input.GetMouseButtonUp(0) )
#else
        for (int i = 0; i < touches.Length; i++)
        {
            currentPotition = Camera.main.ScreenToWorldPoint(touches[i].position);


            if (fingerId == touches[i].fingerId && touches[i].phase == TouchPhase.Ended)
#endif
            {
                FirstTouchSuccess = false;
                startPosition = Vector2.zero;
                endposition = Vector2.zero;
                transform.position = Vector3.zero;
                fingerId = NO_FINGER;
            }

#if MOUSE
            if (Input.GetMouseButtonDown(0))    // eka 
#else
            if (fingerId == NO_FINGER && touches[i].phase == TouchPhase.Began)
#endif
            {
#if !MOUSE
                fingerId = touches[i].fingerId;
#endif

#if MOUSE
                if (touching && !touchingLastFrame && ScreenView.Contains(currentPotition))
#else
                if (touching && !touchingLastFrame && ScreenView.Contains(currentPotition))
#endif
                {
                    startPosition = currentPotition;
                    transform.position = startPosition;
                    touching = true;
                    FirstTouchSuccess = true;
                    position = Player.transform.position;
                }
                else
                {
                    FirstTouchSuccess = false;
                }
            }
            else
            {
                if (touching && FirstTouchSuccess)    // toka
                {
                    if (touching && touchingLastFrame)
                    {
                        endposition = currentPotition;
                        transform.position = endposition;
                    }

                    Vector2 touchVector = GetTouchVector();
                    if (touchVector.magnitude > maxLength && touchingLastFrame)
                    {
                        endposition = startPosition + (touchVector.normalized * maxLength);
                        transform.position = endposition;
                    }
                }
                else
                {
                    touching = false;
                }
            }
#if !MOUSE
        }
#endif
        touchingLastFrame = touching;



    }



    void MouseUpdate()
    {
        // Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        // transform.Translate(lastPos - transform.position);
        Vector3 movVec = Player.transform.position - position;
        startPosition = startPosition + (Vector2)movVec;
        endposition = endposition + (Vector2)movVec;

        position = Player.transform.position;

        touching = Input.GetMouseButton(0);

        Vector3 currentPotition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // sormi
        Vector3 cameraPos = Camera.main.transform.position;
        Rect ScreenView = new Rect(cameraPos.x + HitBox.x, cameraPos.y + HitBox.y, HitBox.width, HitBox.height);

        Base.transform.position = startPosition;

        if (Input.GetMouseButtonUp(0))
        {
            FirstTouchSuccess = false;
            startPosition = Vector2.zero;
            endposition = Vector2.zero;
            transform.position = Vector3.zero;
            fingerId = NO_FINGER;
        }

        if (Input.GetMouseButtonDown(0)) // eka 
        {


            if (touching && !touchingLastFrame && ScreenView.Contains(currentPotition))
            {
                startPosition = currentPotition;
                transform.position = startPosition;
                touching = true;
                FirstTouchSuccess = true;
                position = Player.transform.position;
            }
            else
            {
                FirstTouchSuccess = false;
            }
        }
        else
        {
            if (touching && FirstTouchSuccess)    // toka
            {
                if (touching && touchingLastFrame)
                {
                    endposition = currentPotition;
                    transform.position = endposition;
                }

                Vector2 touchVector = GetTouchVector();
                if (touchVector.magnitude > maxLength && touchingLastFrame)
                {
                    endposition = startPosition + (touchVector.normalized * maxLength);
                    transform.position = endposition;
                }
            }
            else
            {
                touching = false;
            }
        }

        touchingLastFrame = touching;
    }





    void UpdateVisuals()
    {
        renderer.color = touching ? Color.grey : Color.white;
        // normilizoi
    }

    Vector2 GetTouchVector()
    {
        return (endposition - startPosition);
    }

    public Vector2 GetInputVector()
    {
        Vector2 pos = endposition - startPosition;

        if (pos.y != 0 && pos.x != 0)
        {
            if (pos.magnitude < maxLength * 2)
            {
                pos.x *= (Map(pos.magnitude, 0f, maxLength, 0f, 1f)) / 2f;
                pos.y *= (Map(pos.magnitude, 0f, maxLength, 0f, 1f)) / 2f;

                return pos;
            }
        }
        return new Vector2(0f, 0f);
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
